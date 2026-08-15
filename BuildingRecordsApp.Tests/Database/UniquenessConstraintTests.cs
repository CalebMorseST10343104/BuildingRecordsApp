using BuildingRecordsApp.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BuildingRecordsApp.Tests.Database;

public class UniquenessConstraintTests
{
    [Fact]
    public async Task Unit_number_must_be_unique_within_a_building()
    {
        await using var db = await SqliteTestDatabase.CreateAsync();
        var property = EntityFactory.Property();
        var building = EntityFactory.Building(property);
        db.Context.Units.AddRange(EntityFactory.Unit(building, "101"), EntityFactory.Unit(building, "101"));
        await Assert.ThrowsAsync<DbUpdateException>(() => db.Context.SaveChangesAsync());
    }

    [Fact]
    public async Task Same_unit_number_is_allowed_in_different_buildings()
    {
        await using var db = await SqliteTestDatabase.CreateAsync();
        var property = EntityFactory.Property();
        db.Context.Units.AddRange(
            EntityFactory.Unit(EntityFactory.Building(property, "A"), "101"),
            EntityFactory.Unit(EntityFactory.Building(property, "B"), "101"));
        await db.Context.SaveChangesAsync();
        Assert.Equal(2, await db.Context.Units.CountAsync());
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task Bay_and_storeroom_numbers_are_unique_within_a_property(bool parkingBay)
    {
        await using var db = await SqliteTestDatabase.CreateAsync();
        var property = EntityFactory.Property();
        if (parkingBay)
            db.Context.ParkingBays.AddRange(new() { Property = property, ParkingBayNumber = "1" }, new() { Property = property, ParkingBayNumber = "1" });
        else
            db.Context.StoreRooms.AddRange(new() { Property = property, StoreRoomNumber = "1" }, new() { Property = property, StoreRoomNumber = "1" });
        await Assert.ThrowsAsync<DbUpdateException>(() => db.Context.SaveChangesAsync());
    }

    [Fact]
    public async Task Vehicle_registration_is_unique()
    {
        await using var db = await SqliteTestDatabase.CreateAsync();
        db.Context.Vehicles.AddRange(new() { VehicleRegistration = "CA123" }, new() { VehicleRegistration = "CA123" });
        await Assert.ThrowsAsync<DbUpdateException>(() => db.Context.SaveChangesAsync());
    }
}
