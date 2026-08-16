using System.Net;
using System.Text.RegularExpressions;
using BuildingRecordsApp.Models.Entities;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;

namespace BuildingRecordsApp.Tests.Integration;

public class RazorPageJourneyTests
{
    [Fact]
    public async Task Export_page_downloads_a_scoped_excel_workbook()
    {
        using var factory = new BuildingRecordsWebApplicationFactory();
        using var client = Client(factory);
        var ids = await SeedBareUnitAsync(factory, "EX1", "Export Building", "Export Property");

        var page = await client.GetStringAsync("/Exports");
        Assert.Contains("Download Excel workbook", page);
        Assert.Contains("Export Property", page);
        Assert.Contains("Export Building", page);

        var response = await PostFormAsync(client, "/Exports", new()
        {
            ["PropertyId"] = ids.PropertyId.ToString(),
            ["BuildingIds"] = ids.BuildingId.ToString()
        });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", response.Content.Headers.ContentType?.MediaType);
        Assert.EndsWith(".xlsx", response.Content.Headers.ContentDisposition?.FileNameStar);
        var bytes = await response.Content.ReadAsByteArrayAsync();
        Assert.True(bytes.Length > 1000);
        Assert.Equal("PK", System.Text.Encoding.ASCII.GetString(bytes, 0, 2));
    }

    [Fact]
    public async Task Export_page_requires_a_property()
    {
        using var factory = new BuildingRecordsWebApplicationFactory();
        using var client = Client(factory);

        var response = await PostFormAsync(client, "/Exports", []);
        var html = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("Select a property to export.", html);
    }

    [Fact]
    public async Task Index_tables_use_readable_headings_relationship_filters_and_separate_action_strips()
    {
        using var factory = new BuildingRecordsWebApplicationFactory();
        using var client = Client(factory);
        var ids = await SeedBareUnitAsync(factory, "F1", "Filter Building", "Filter Property");

        var buildings = await client.GetStringAsync("/Buildings");
        Assert.Contains(">Property</th>", buildings);
        Assert.Contains(">Building Name</th>", buildings);
        Assert.DoesNotContain(">PropertyName</th>", buildings);
        Assert.Contains("Filter Property", buildings);
        Assert.Contains("data-column-filter=\"0\"", buildings);
        Assert.DoesNotContain(">Actions</th>", buildings);
        Assert.Contains($"data-record-actions=\"{ids.BuildingId}\"", buildings);
        Assert.Contains($"/Buildings/Edit?id={ids.BuildingId}", buildings);
        Assert.Contains($"/Buildings/Delete?id={ids.BuildingId}", buildings);

        var units = await client.GetStringAsync("/Units");
        Assert.Contains($"/Units/Details?id={ids.UnitId}", units);
        Assert.Contains(">Open</a>", units);
    }

    [Fact]
    public async Task Custom_property_and_access_count_indexes_expose_filters_without_action_cells()
    {
        using var factory = new BuildingRecordsWebApplicationFactory();
        using var client = Client(factory);
        var ids = await SeedBareUnitAsync(factory, "FC1", "Custom Filter Building", "Custom Filter Property");
        await factory.WithDatabaseAsync(async context =>
        {
            context.AccessDeviceCounts.Add(new AccessDeviceCount { UnitId = ids.UnitId });
            await context.SaveChangesAsync();
        });

        var properties = await client.GetStringAsync("/Properties");
        Assert.Contains("data-card-filter=\"[data-property-card]\"", properties);
        Assert.Contains("data-property-card", properties);

        var counts = await client.GetStringAsync("/AccessDeviceCounts");
        Assert.Contains("aria-label=\"Filter by building\"", counts);
        Assert.Contains("aria-label=\"Filter by unit\"", counts);
        Assert.DoesNotContain(">Action</th>", counts);
        Assert.DoesNotContain("/AccessDeviceCounts/Delete", counts);
        Assert.Contains("index-actions-cell", counts);
    }

    [Fact]
    public async Task Ownership_contact_actions_use_the_working_route_and_edit_page_opens()
    {
        using var factory = new BuildingRecordsWebApplicationFactory();
        using var client = Client(factory);
        var ids = await SeedOwnedUnitAsync(factory);
        var contactId = 0;
        await factory.WithDatabaseAsync(async context =>
        {
            var person = new Person { FirstName = "Route", LastName = "Contact" };
            var contact = new OwnershipContact { OwnershipId = ids.OwnershipId, Person = person };
            context.Add(contact);
            await context.SaveChangesAsync();
            contactId = contact.OwnershipContactId;
        });

        var index = await client.GetStringAsync("/OwnershipContacts");
        var edit = await client.GetAsync($"/OwnershipContacts/Edit?id={contactId}");

        Assert.Contains($"/OwnershipContacts/Edit?id={contactId}", index);
        Assert.DoesNotContain("/Ownership%20Contacts", index);
        Assert.Contains("Ownership Building", index);
        Assert.Contains("O1", index);
        Assert.Equal(HttpStatusCode.OK, edit.StatusCode);
    }

    [Fact]
    public async Task Organization_registration_is_saved_on_create_and_edit_and_delete_details_are_readable()
    {
        using var factory = new BuildingRecordsWebApplicationFactory();
        using var client = Client(factory);

        var create = await PostFormAsync(client, "/Organizations/Create", new()
        {
            ["ViewModel.Name"] = "Registration Test Trust",
            ["ViewModel.Address"] = "First address",
            ["ViewModel.RegistrationNumber"] = "TRUST-100"
        });
        Assert.Equal(HttpStatusCode.Redirect, create.StatusCode);

        var organizationId = 0;
        await factory.WithDatabaseAsync(async context =>
        {
            var organization = await context.Organizations.SingleAsync(o => o.Name == "Registration Test Trust");
            organizationId = organization.OrganizationId;
            Assert.Equal("TRUST-100", organization.RegistrationReference);
        });

        var edit = await PostFormAsync(client, $"/Organizations/Edit?id={organizationId}", new()
        {
            ["ViewModel.OrganizationId"] = organizationId.ToString(),
            ["ViewModel.Name"] = "Registration Test Trust",
            ["ViewModel.Address"] = "Updated address",
            ["ViewModel.RegistrationNumber"] = "TRUST-200"
        });
        Assert.Equal(HttpStatusCode.Redirect, edit.StatusCode);
        await factory.WithDatabaseAsync(async context =>
            Assert.Equal("TRUST-200", await context.Organizations.Where(o => o.OrganizationId == organizationId)
                .Select(o => o.RegistrationReference).SingleAsync()));

        var delete = await client.GetStringAsync($"/Organizations/Delete?id={organizationId}");
        Assert.Contains("Registration Test Trust", delete);
        Assert.Contains("TRUST-200", delete);
        Assert.DoesNotContain("System.Func", delete);
    }

    [Fact]
    public async Task Juristic_ownership_without_an_organization_shows_a_field_error()
    {
        using var factory = new BuildingRecordsWebApplicationFactory();
        using var client = Client(factory);
        var ids = await SeedBareUnitAsync(factory, "J1");

        var response = await PostFormAsync(client, "/Ownerships/Create", new()
        {
            ["ViewModel.UnitId"] = ids.UnitId.ToString(),
            ["ViewModel.OwnershipType"] = "Juristic"
        });
        var html = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("Select a company or trust for juristic ownership.", html);
        Assert.Contains("data-valmsg-for=\"ViewModel.OrganizationId\"", html);
    }

    [Fact]
    public async Task Lease_edit_uses_dates_only_and_cannot_move_to_another_unit()
    {
        using var factory = new BuildingRecordsWebApplicationFactory();
        using var client = Client(factory);
        var first = await SeedBareUnitAsync(factory, "L1");
        var second = await SeedBareUnitAsync(factory, "L2", "Second Lease Building");
        var leaseId = 0;
        await factory.WithDatabaseAsync(async context =>
        {
            var lease = new Lease
            {
                UnitId = first.UnitId,
                LeaseHolderName = "Lease Holder",
                StartDate = new DateTime(2026, 8, 1),
                EndDate = new DateTime(2027, 7, 31)
            };
            context.Add(lease);
            await context.SaveChangesAsync();
            leaseId = lease.LeaseId;
        });

        var page = await client.GetStringAsync($"/Leases/Edit?id={leaseId}");
        Assert.Contains("A lease summary cannot be moved to another unit.", page);
        Assert.Contains("type=\"date\"", page);
        Assert.Contains(" disabled", page);

        var tampered = await PostFormAsync(client, $"/Leases/Edit?id={leaseId}", new()
        {
            ["ViewModel.LeaseId"] = leaseId.ToString(),
            ["ViewModel.UnitId"] = second.UnitId.ToString(),
            ["ViewModel.LeaseHolderName"] = "Lease Holder",
            ["ViewModel.StartDate"] = "2026-08-01",
            ["ViewModel.EndDate"] = "2027-07-31",
            ["ViewModel.DeclaredOccupantCount"] = "1"
        });
        var tamperedHtml = await tampered.Content.ReadAsStringAsync();
        Assert.Equal(HttpStatusCode.OK, tampered.StatusCode);
        Assert.Contains("cannot be moved", tamperedHtml);
        await factory.WithDatabaseAsync(async context =>
            Assert.Equal(first.UnitId, await context.Leases.Where(l => l.LeaseId == leaseId).Select(l => l.UnitId).SingleAsync()));
    }

    [Fact]
    public async Task Property_context_filters_allocations_and_returns_to_property_after_create()
    {
        using var factory = new BuildingRecordsWebApplicationFactory();
        using var client = Client(factory);
        var first = await SeedBareUnitAsync(factory, "P1", "First Property Building", "First Property");
        var second = await SeedBareUnitAsync(factory, "P2", "Second Property Building", "Second Property");

        var parkingPage = await client.GetStringAsync($"/ParkingBays/Create?propertyId={first.PropertyId}");
        Assert.Contains("[First Property Building] P1", parkingPage);
        Assert.DoesNotContain("[Second Property Building] P2", parkingPage);

        var parking = await PostFormAsync(client, $"/ParkingBays/Create?propertyId={first.PropertyId}", new()
        {
            ["ReturnToProperty"] = "true",
            ["ViewModel.PropertyId"] = first.PropertyId.ToString(),
            ["ViewModel.ParkingBayNumber"] = "PB-TEST",
            ["ViewModel.UnitID"] = first.UnitId.ToString()
        });
        Assert.Equal($"/Properties/Details?id={first.PropertyId}", parking.Headers.Location?.OriginalString);

        var storeroom = await PostFormAsync(client, $"/StoreRooms/Create?propertyId={first.PropertyId}", new()
        {
            ["ReturnToProperty"] = "true",
            ["ViewModel.PropertyId"] = first.PropertyId.ToString(),
            ["ViewModel.StoreRoomNumber"] = "SR-TEST",
            ["ViewModel.UnitId"] = first.UnitId.ToString()
        });
        Assert.Equal($"/Properties/Details?id={first.PropertyId}", storeroom.Headers.Location?.OriginalString);

        var unitsJson = await client.GetStringAsync($"/ParkingBays/Create?handler=Units&propertyId={second.PropertyId}");
        Assert.Contains("P2", unitsJson);
        Assert.DoesNotContain("P1", unitsJson);
    }

    [Fact]
    public async Task Unit_creation_filters_buildings_by_property()
    {
        using var factory = new BuildingRecordsWebApplicationFactory();
        using var client = Client(factory);
        var first = await SeedBareUnitAsync(factory, "U1", "Allowed Building", "Allowed Property");
        await SeedBareUnitAsync(factory, "U2", "Hidden Building", "Hidden Property");

        var page = await client.GetStringAsync($"/Units/Create?propertyId={first.PropertyId}");
        var json = await client.GetStringAsync($"/Units/Create?handler=Buildings&propertyId={first.PropertyId}");

        Assert.Contains("Allowed Building", page);
        Assert.DoesNotContain("Hidden Building", page);
        Assert.Contains("Allowed Building", json);
        Assert.DoesNotContain("Hidden Building", json);
    }

    [Fact]
    public async Task Access_counts_have_grouped_headers_and_no_delete_action_or_resubmission()
    {
        using var factory = new BuildingRecordsWebApplicationFactory();
        using var client = Client(factory);
        var ids = await SeedBareUnitAsync(factory, "A1");
        var countId = 0;
        await factory.WithDatabaseAsync(async context =>
        {
            var counts = new AccessDeviceCount { UnitId = ids.UnitId, OwnershipContactTagCount = 1 };
            context.Add(counts);
            await context.SaveChangesAsync();
            countId = counts.AccessDeviceCountId;
        });

        var index = await client.GetStringAsync("/AccessDeviceCounts");
        Assert.Contains("colspan=\"2\"", index);
        Assert.Contains("Ownership contacts", index);
        Assert.DoesNotContain($"/AccessDeviceCounts/Delete?id={countId}", index);

        var delete = await client.GetStringAsync($"/AccessDeviceCounts/Delete?id={countId}");
        Assert.Contains("cannot be deleted separately", delete);
        Assert.DoesNotContain("<form method=\"post\"", delete);

    }

    [Fact]
    public async Task Occupancy_type_is_a_controlled_selection()
    {
        using var factory = new BuildingRecordsWebApplicationFactory();
        using var client = Client(factory);

        var html = await client.GetStringAsync("/Occupancies/Create");

        Assert.Contains("name=\"ViewModel.OccupationType\"", html);
        foreach (var occupationType in new[]
        {
            "Owner", "Owner &amp; Long-Term Letting", "Owner &amp; Short-Term Letting", "Owner Family",
            "Short-Term Letting", "Tenant", "Tenant &amp; Short-Term Letting"
        })
            Assert.Contains(occupationType, html);
        Assert.DoesNotContain("<input class=\"form-control\" type=\"text\" id=\"ViewModel_OccupationType\"", html);
    }

    [Fact]
    public async Task Dashboard_issue_appears_after_create_and_disappears_after_edit()
    {
        using var factory = new BuildingRecordsWebApplicationFactory();
        using var client = Client(factory);

        var create = await PostFormAsync(client, "/Properties/Create", new()
        {
            ["ViewModel.Name"] = "Integration Property",
            ["ViewModel.Address"] = ""
        });
        Assert.Equal(HttpStatusCode.Redirect, create.StatusCode);

        int propertyId = 0;
        await factory.WithDatabaseAsync(async context =>
            propertyId = await context.Properties.Where(p => p.Name == "Integration Property").Select(p => p.PropertyId).SingleAsync());

        var followUpBefore = await client.GetStringAsync("/FollowUp");
        var homeBefore = await client.GetStringAsync("/");
        Assert.Contains("Property address or description is missing.", followUpBefore);
        Assert.Contains("1 item needs attention", homeBefore);

        var edit = await PostFormAsync(client, $"/Properties/Edit?id={propertyId}", new()
        {
            ["ViewModel.PropertyId"] = propertyId.ToString(),
            ["ViewModel.Name"] = "Integration Property",
            ["ViewModel.Address"] = "Now complete"
        });
        Assert.Equal(HttpStatusCode.Redirect, edit.StatusCode);

        var followUpAfter = await client.GetStringAsync("/FollowUp");
        var homeAfter = await client.GetStringAsync("/");
        Assert.Contains("Everything is up to date", followUpAfter);
        Assert.DoesNotContain("Property address or description is missing.", followUpAfter);
        Assert.Contains("Nothing needs attention", homeAfter);
    }

    [Fact]
    public async Task Invalid_create_redisplays_field_error_entered_value_and_dropdown()
    {
        using var factory = new BuildingRecordsWebApplicationFactory();
        using var client = Client(factory);

        var response = await PostFormAsync(client, "/Units/Create", new()
        {
            ["ViewModel.UnitNumber"] = "Remember me"
        });
        var html = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("Building is required.", html);
        Assert.Contains("value=\"Remember me\"", html);
        Assert.Contains("name=\"ViewModel.BuildingId\"", html);
        Assert.DoesNotContain("We couldn't complete that change", html);
    }

    [Fact]
    public async Task Dashboard_severity_filter_can_produce_a_clear_no_matches_state()
    {
        using var factory = new BuildingRecordsWebApplicationFactory();
        using var client = Client(factory);
        await factory.WithDatabaseAsync(async context =>
        {
            context.Properties.Add(new Property { Name = "Address Missing", Address = "" });
            await context.SaveChangesAsync();
        });

        var urgent = await client.GetStringAsync("/FollowUp?Severity=Urgent");
        var important = await client.GetStringAsync("/FollowUp?Severity=Important");

        Assert.Contains("No items match these filters", urgent);
        Assert.Contains("There are still 1 follow-up items", urgent);
        Assert.Contains("Property address or description is missing.", important);
        Assert.Contains("Showing 1 of 1 follow-up item", important);
    }

    [Fact]
    public async Task Every_index_and_create_page_renders_without_a_raw_error()
    {
        using var factory = new BuildingRecordsWebApplicationFactory();
        using var client = Client(factory);
        string[] pages =
        [
            "AccessDeviceCounts", "AgentCompanies", "Agents", "Buildings", "Leases",
            "Occupancies", "Organizations", "OwnershipContacts", "Ownerships", "ParkingBays",
            "Persons", "Properties", "StoreRooms", "Units", "Vehicles"
        ];

        foreach (var page in pages)
        {
            foreach (var action in new[] { "Index", "Create" })
            {
                var response = await client.GetAsync($"/{page}/{action}");
                var html = await response.Content.ReadAsStringAsync();

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                if (action == "Create")
                    Assert.Contains("name=\"ViewModel.", html);
                Assert.DoesNotContain("We couldn't complete that change", html);
            }
        }
    }

    private static HttpClient Client(BuildingRecordsWebApplicationFactory factory) =>
        factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

    private static async Task<HttpResponseMessage> PostFormAsync(HttpClient client, string path, Dictionary<string, string> fields)
    {
        var getResponse = await client.GetAsync(path);
        getResponse.EnsureSuccessStatusCode();
        var html = await getResponse.Content.ReadAsStringAsync();
        var match = Regex.Match(html, "name=\"__RequestVerificationToken\" type=\"hidden\" value=\"([^\"]+)\"");
        Assert.True(match.Success, $"No antiforgery token was rendered by {path}.");
        fields["__RequestVerificationToken"] = WebUtility.HtmlDecode(match.Groups[1].Value);

        return await client.PostAsync(path, new FormUrlEncodedContent(fields));
    }

    private static async Task<TestRecordIds> SeedBareUnitAsync(
        BuildingRecordsWebApplicationFactory factory,
        string unitNumber,
        string buildingName = "Integration Building",
        string propertyName = "Integration Property")
    {
        var result = new TestRecordIds();
        await factory.WithDatabaseAsync(async context =>
        {
            var property = new Property { Name = $"{propertyName} {Guid.NewGuid():N}", Address = "Complete" };
            var building = new Building
            {
                Property = property,
                Name = buildingName,
                Address = "Complete",
                NumberOfFloors = 1,
                NumberOfUnits = 1
            };
            var unit = new Unit { Building = building, UnitNumber = unitNumber };
            context.Add(unit);
            await context.SaveChangesAsync();
            result = new TestRecordIds(property.PropertyId, building.BuildingId, unit.UnitId, 0);
        });
        return result;
    }

    private static async Task<TestRecordIds> SeedOwnedUnitAsync(BuildingRecordsWebApplicationFactory factory)
    {
        var ids = await SeedBareUnitAsync(factory, "O1", "Ownership Building", "Ownership Property");
        await factory.WithDatabaseAsync(async context =>
        {
            var ownership = new Ownership { UnitId = ids.UnitId, OwnershipType = "Natural" };
            context.Add(ownership);
            await context.SaveChangesAsync();
            ids = ids with { OwnershipId = ownership.OwnershipId };
        });
        return ids;
    }

    private sealed record TestRecordIds(int PropertyId = 0, int BuildingId = 0, int UnitId = 0, int OwnershipId = 0);
}
