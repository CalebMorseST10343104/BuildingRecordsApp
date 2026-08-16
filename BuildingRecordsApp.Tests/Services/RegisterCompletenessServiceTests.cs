using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Services;
using BuildingRecordsApp.Tests.Database;

namespace BuildingRecordsApp.Tests.Services;

public class RegisterCompletenessServiceTests
{
    private static readonly DateTimeOffset Today = new(2026, 8, 16, 12, 0, 0, TimeSpan.Zero);

    [Fact]
    public async Task Incomplete_unit_reports_urgent_relationship_issues_with_context_and_actions()
    {
        await using var db = await SqliteTestDatabase.CreateAsync();
        var property = EntityFactory.Property();
        var building = CompleteBuilding(property);
        var unit = EntityFactory.Unit(building, "101");
        db.Context.Add(unit);
        await db.Context.SaveChangesAsync();

        var issues = await Service(db.Context).GetIssuesAsync();

        var unitIssues = issues.Where(i => i.RecordType == RegisterRecordType.Unit).ToList();
        Assert.Equal(
            ["unit.access-count.missing", "unit.ownership.missing", "unit.primary-contact.missing"],
            unitIssues.Select(i => i.Code).Order().ToArray());
        Assert.All(unitIssues, issue => Assert.Equal(CompletenessSeverity.Urgent, issue.Severity));
        Assert.All(unitIssues, issue =>
        {
            Assert.Equal("Chelsea", issue.PropertyName);
            Assert.Equal("A", issue.BuildingName);
            Assert.Equal("101", issue.UnitNumber);
            Assert.Contains($"{unit.UnitId}", issue.ActionUrl);
        });
    }

    [Fact]
    public async Task Complete_core_register_does_not_flag_genuinely_optional_absences_or_zero_counts()
    {
        await using var db = await SqliteTestDatabase.CreateAsync();
        var property = EntityFactory.Property();
        var building = CompleteBuilding(property);
        var contact = EntityFactory.Person("Primary");
        contact.PhoneNumber = "0215550100";
        var unit = EntityFactory.Unit(building, "101");
        unit.PrimaryContactPerson = contact;
        unit.AccessDeviceCount = new AccessDeviceCount
        {
            OwnershipContactTagCount = 0,
            OwnershipContactRemoteCount = 0,
            OccupantTagCount = 0,
            OccupantRemoteCount = 0,
            AgentTagCount = 0,
            AgentRemoteCount = 0
        };
        var ownership = new Ownership { Unit = unit, OwnershipType = "Natural" };
        ownership.OwnershipContacts.Add(new OwnershipContact { Ownership = ownership, Person = contact });
        db.Context.AddRange(unit, ownership);
        await db.Context.SaveChangesAsync();

        var issues = await Service(db.Context).GetIssuesAsync();

        Assert.Empty(issues);
    }

    [Fact]
    public async Task Contactability_is_reported_once_even_when_person_fills_multiple_roles()
    {
        await using var db = await SqliteTestDatabase.CreateAsync();
        var building = CompleteBuilding(EntityFactory.Property());
        var person = EntityFactory.Person("Shared");
        var unit = EntityFactory.Unit(building, "101");
        unit.PrimaryContactPerson = person;
        unit.AccessDeviceCount = CompleteAccessCount();
        var ownership = new Ownership { Unit = unit, OwnershipType = "Natural" };
        ownership.OwnershipContacts.Add(new OwnershipContact { Ownership = ownership, Person = person });
        db.Context.AddRange(unit, ownership);
        await db.Context.SaveChangesAsync();

        var issues = await Service(db.Context).GetIssuesAsync();

        var issue = Assert.Single(issues, i => i.Code == "person.contact-method.missing");
        Assert.Contains("primary contact", issue.Summary);
        Assert.Contains("ownership contact", issue.Summary);
        Assert.Equal(unit.UnitId, issue.UnitId);
    }

    [Fact]
    public async Task Current_operational_records_report_documented_important_and_time_sensitive_issues()
    {
        await using var db = await SqliteTestDatabase.CreateAsync();
        var building = CompleteBuilding(EntityFactory.Property());
        var contact = EntityFactory.Person("Contact");
        contact.Email = "contact@example.com";
        var unit = EntityFactory.Unit(building, "101");
        unit.PrimaryContactPerson = contact;
        unit.AccessDeviceCount = new AccessDeviceCount { OwnershipContactTagCount = 0 };
        var ownership = new Ownership { Unit = unit, OwnershipType = "Natural" };
        ownership.OwnershipContacts.Add(new OwnershipContact { Ownership = ownership, Person = contact });
        unit.Lease = new Lease
        {
            LeaseHolderName = "Tenant",
            StartDate = Today.AddYears(-1).Date,
            EndDate = Today.AddDays(-1).Date,
            DeclaredOccupantCount = 1
        };
        unit.Occupants.Add(new Occupancy { Occupant = EntityFactory.Person("Occupant") });
        unit.Vehicles.Add(new Vehicle { VehicleRegistration = "CA 123", VehicleMake = "Toyota" });
        db.Context.AddRange(unit, ownership);
        await db.Context.SaveChangesAsync();

        var issues = await Service(db.Context).GetIssuesAsync();
        var codes = issues.Select(i => i.Code).ToHashSet();

        Assert.Contains("access-count.unknown", codes);
        Assert.Contains("lease.expired", codes);
        Assert.Contains("lease.emergency-contact.missing", codes);
        Assert.Contains("occupancy.type.missing", codes);
        Assert.Contains("vehicle.description.incomplete", codes);
        Assert.DoesNotContain("unit.ownership.missing", codes);
    }

    [Fact]
    public async Task Organization_and_agent_company_details_are_only_required_when_currently_relevant()
    {
        await using var db = await SqliteTestDatabase.CreateAsync();
        var unusedOrganization = new Organization { Name = "Unused" };
        var unusedCompany = new AgentCompany { CompanyName = "Unused agency" };
        var building = CompleteBuilding(EntityFactory.Property());
        var unit = EntityFactory.Unit(building, "101");
        var organization = new Organization { Name = "Current owner" };
        unit.Ownership = new Ownership { OwnershipType = "Juristic", Organization = organization };
        unit.AccessDeviceCount = CompleteAccessCount();
        var contact = EntityFactory.Person("Contact");
        contact.PhoneNumber = "0215550100";
        unit.PrimaryContactPerson = contact;
        unit.Ownership.OwnershipContacts.Add(new OwnershipContact { Person = contact });
        var company = new AgentCompany { CompanyName = "Current agency" };
        company.Agents.Add(new Agent { Person = EntityFactory.Person("Agent") });
        company.Agents.Single().Person.Email = "agent@example.com";
        db.Context.AddRange(unusedOrganization, unusedCompany, unit, company);
        await db.Context.SaveChangesAsync();

        var issues = await Service(db.Context).GetIssuesAsync();

        Assert.Equal(2, issues.Count(i => i.RecordType == RegisterRecordType.Organization && i.RecordId == organization.OrganizationId));
        Assert.Equal(2, issues.Count(i => i.RecordType == RegisterRecordType.AgentCompany && i.RecordId == company.AgentCompanyId));
        Assert.DoesNotContain(issues, i => i.RecordType == RegisterRecordType.Organization && i.RecordId == unusedOrganization.OrganizationId);
        Assert.DoesNotContain(issues, i => i.RecordType == RegisterRecordType.AgentCompany && i.RecordId == unusedCompany.AgentCompanyId);
    }

    [Fact]
    public async Task Physical_contact_and_ownership_legacy_gaps_are_all_reported()
    {
        await using var db = await SqliteTestDatabase.CreateAsync();
        var property = new Property { Name = "Chelsea" };
        var building = new Building { Property = property, Name = "A" };
        var unnamed = new Person { PhoneNumber = "0215550100" };
        var unit = EntityFactory.Unit(building, "101");
        unit.PrimaryContactPerson = unnamed;
        unit.AccessDeviceCount = CompleteAccessCount();
        var ownership = new Ownership { Unit = unit, OwnershipType = "Natural" };
        db.Context.AddRange(unit, ownership);
        await db.Context.SaveChangesAsync();

        var codes = (await Service(db.Context).GetIssuesAsync()).Select(i => i.Code).ToHashSet();

        Assert.Contains("property.address.missing", codes);
        Assert.Contains("building.address.missing", codes);
        Assert.Contains("building.expected-units.unknown", codes);
        Assert.Contains("building.floors.unknown", codes);
        Assert.Contains("person.name.missing", codes);
        Assert.Contains("ownership.contacts.missing", codes);
    }

    private static RegisterCompletenessService Service(BuildingContext context) =>
        new(context, new FixedTimeProvider(Today));

    private static Building CompleteBuilding(Property property) => new()
    {
        Property = property,
        Name = "A",
        Address = "Test address",
        NumberOfUnits = 10,
        NumberOfFloors = 5
    };

    private static AccessDeviceCount CompleteAccessCount() => new()
    {
        OwnershipContactTagCount = 0,
        OwnershipContactRemoteCount = 0,
        OccupantTagCount = 0,
        OccupantRemoteCount = 0,
        AgentTagCount = 0,
        AgentRemoteCount = 0
    };

    private sealed class FixedTimeProvider(DateTimeOffset now) : TimeProvider
    {
        public override DateTimeOffset GetUtcNow() => now;
        public override TimeZoneInfo LocalTimeZone => TimeZoneInfo.Utc;
    }
}
