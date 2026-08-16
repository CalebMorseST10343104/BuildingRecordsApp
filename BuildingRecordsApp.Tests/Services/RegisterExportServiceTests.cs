using System.IO.Compression;
using System.Text;
using BuildingRecordsApp.Data;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models;
using BuildingRecordsApp.Services;
using Microsoft.EntityFrameworkCore;

namespace BuildingRecordsApp.Tests.Services;

public sealed class RegisterExportServiceTests
{
    [Fact]
    public async Task Export_contains_the_expected_sheets_and_only_selected_buildings()
    {
        var options = new DbContextOptionsBuilder<BuildingContext>().UseSqlite("Data Source=:memory:").Options;
        await using var context = new BuildingContext(options);
        await context.Database.OpenConnectionAsync();
        await context.Database.EnsureCreatedAsync();

        var property = new Property { Name = "Export Property", Address = "Test" };
        var included = new Building { Name = "Included Building", Address = "One", NumberOfUnits = 1, Property = property };
        var excluded = new Building { Name = "Excluded Building", Address = "Two", NumberOfUnits = 1, Property = property };
        included.Units.Add(new Unit
        {
            UnitNumber = "A1",
            Building = included,
            Bedrooms = 2,
            Lease = new Lease
            {
                LeaseHolderName = "Test Holder",
                StartDate = new DateTime(2026, 1, 1),
                EndDate = new DateTime(2026, 12, 31),
                SignedRules = false
            },
            Vehicles =
            [
                new Vehicle { VehicleRegistration = "TEST1", VehicleColor = "Red", VehicleMake = "Corolla", VehicleModel = "Toyota" }
            ]
        });
        excluded.Units.Add(new Unit { UnitNumber = "B1", Building = excluded, Bedrooms = 1 });
        context.AddRange(property, included, excluded);
        await context.SaveChangesAsync();

        var result = await new RegisterExportService(context).ExportExcelAsync(property.PropertyId, [included.BuildingId]);

        Assert.StartsWith("PK", Encoding.ASCII.GetString(result.Content, 0, 2));
        Assert.EndsWith(".xlsx", result.FileName);
        using var archive = new ZipArchive(new MemoryStream(result.Content), ZipArchiveMode.Read);
        var workbook = Read(archive, "xl/workbook.xml");
        foreach (var name in new[] { "MAIN", "PEOPLE", "NATURAL OWNERSHIP", "JURISTIC OWNERSHIP", "AGENTS", "OCCUPANTS", "LEASES", "VEHICLES", "TAG AND REMOTE", "BASEMENT" })
            Assert.Contains($"name=\"{name}\"", workbook);
        var main = Read(archive, "xl/worksheets/sheet1.xml");
        Assert.Contains("Included Building", main);
        Assert.Contains("A1", main);
        Assert.DoesNotContain("Excluded Building", main);
        Assert.DoesNotContain("B1", main);
        Assert.Contains("mergeCell", main);
        Assert.Contains("autoFilter", main);
        Assert.Contains("conditionalFormatting", main);
        Assert.Contains("Red Corolla Toyota", main);
        Assert.DoesNotContain("Red, Corolla, Toyota", main);
        var styles = Read(archive, "xl/styles.xml");
        Assert.Contains("FFFFF2CC", styles);
        Assert.Contains("FFFFC7CE", styles);
    }

    [Fact]
    public void Occupation_types_match_the_original_register_choices()
    {
        Assert.Equal(
        [
            "Owner",
            "Owner & Long-Term Letting",
            "Owner & Short-Term Letting",
            "Owner Family",
            "Short-Term Letting",
            "Tenant",
            "Tenant & Short-Term Letting"
        ], OccupancyTypes.All);
    }

    [Fact]
    public async Task Export_rejects_a_building_from_another_property()
    {
        var options = new DbContextOptionsBuilder<BuildingContext>().UseSqlite("Data Source=:memory:").Options;
        await using var context = new BuildingContext(options);
        await context.Database.OpenConnectionAsync();
        await context.Database.EnsureCreatedAsync();
        var first = new Property { Name = "First", Address = "One" };
        var second = new Property { Name = "Second", Address = "Two" };
        var foreignBuilding = new Building { Name = "Foreign", Address = "Two", Property = second };
        context.AddRange(first, foreignBuilding);
        await context.SaveChangesAsync();

        await Assert.ThrowsAsync<ArgumentException>(() =>
            new RegisterExportService(context).ExportExcelAsync(first.PropertyId, [foreignBuilding.BuildingId]));
    }

    private static string Read(ZipArchive archive, string path)
    {
        using var reader = new StreamReader(archive.GetEntry(path)!.Open());
        return reader.ReadToEnd();
    }
}
