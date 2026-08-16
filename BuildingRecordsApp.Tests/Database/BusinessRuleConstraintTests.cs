using BuildingRecordsApp.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BuildingRecordsApp.Tests.Database;

public class BusinessRuleConstraintTests
{
    [Fact]
    public async Task Natural_ownership_cannot_reference_an_organization()
    {
        await using var db = await SqliteTestDatabase.CreateAsync();
        db.Context.Ownerships.Add(new()
        {
            Unit = EntityFactory.Unit(EntityFactory.Building(EntityFactory.Property()), "1"),
            OwnershipType = "Natural",
            Organization = new() { Name = "Not allowed" }
        });
        await Assert.ThrowsAsync<DbUpdateException>(() => db.Context.SaveChangesAsync());
    }

    [Fact]
    public async Task Juristic_ownership_requires_an_organization()
    {
        await using var db = await SqliteTestDatabase.CreateAsync();
        db.Context.Ownerships.Add(new()
        {
            Unit = EntityFactory.Unit(EntityFactory.Building(EntityFactory.Property()), "1"),
            OwnershipType = "Juristic"
        });
        await Assert.ThrowsAsync<DbUpdateException>(() => db.Context.SaveChangesAsync());
    }

    [Fact]
    public async Task Access_device_counts_can_be_unknown_or_zero_but_not_negative()
    {
        await using var db = await SqliteTestDatabase.CreateAsync();
        var building = EntityFactory.Building(EntityFactory.Property());
        db.Context.AccessDeviceCounts.AddRange(
            new() { Unit = EntityFactory.Unit(building, "1"), OwnershipContactTagCount = null },
            new() { Unit = EntityFactory.Unit(building, "2"), OwnershipContactTagCount = 0 });
        await db.Context.SaveChangesAsync();

        db.Context.AccessDeviceCounts.Add(new() { Unit = EntityFactory.Unit(building, "3"), OwnershipContactTagCount = -1 });
        await Assert.ThrowsAsync<DbUpdateException>(() => db.Context.SaveChangesAsync());
    }

    [Fact]
    public async Task Deleting_a_unit_cascades_current_dependent_records()
    {
        await using var db = await SqliteTestDatabase.CreateAsync();
        var unit = EntityFactory.Unit(EntityFactory.Building(EntityFactory.Property()), "1");
        var person = EntityFactory.Person("Occupant");
        unit.Lease = new();
        unit.Ownership = new() { OwnershipType = "Natural" };
        unit.AccessDeviceCount = new();
        unit.Occupants.Add(new() { Occupant = person });
        unit.Vehicles.Add(new() { VehicleRegistration = "TEST1" });
        db.Context.Add(unit);
        await db.Context.SaveChangesAsync();

        db.Context.Remove(unit);
        await db.Context.SaveChangesAsync();

        Assert.Empty(await db.Context.Leases.ToListAsync());
        Assert.Empty(await db.Context.Ownerships.ToListAsync());
        Assert.Empty(await db.Context.AccessDeviceCounts.ToListAsync());
        Assert.Empty(await db.Context.Occupancies.ToListAsync());
        Assert.Empty(await db.Context.Vehicles.ToListAsync());
    }
}
