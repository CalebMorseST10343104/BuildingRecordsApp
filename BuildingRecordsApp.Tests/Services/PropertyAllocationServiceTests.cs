using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Services;
using BuildingRecordsApp.Tests.Database;

namespace BuildingRecordsApp.Tests.Services;

public class PropertyAllocationServiceTests
{
    [Fact]
    public async Task Bay_can_be_allocated_and_unallocated_within_its_property()
    {
        await using var db = await SqliteTestDatabase.CreateAsync();
        var property = EntityFactory.Property();
        var unit = EntityFactory.Unit(EntityFactory.Building(property), "1");
        var bay = new ParkingBay { Property = property, ParkingBayNumber = "P1" };
        db.Context.AddRange(unit, bay);
        await db.Context.SaveChangesAsync();
        var service = new PropertyAllocationService(db.Context);

        await service.AllocateParkingBayAsync(bay.ParkingBayId, unit.UnitId);
        Assert.Equal(unit.UnitId, bay.UnitID);
        await service.AllocateParkingBayAsync(bay.ParkingBayId, null);
        Assert.Null(bay.UnitID);
    }

    [Fact]
    public async Task Bay_and_storeroom_cannot_be_allocated_across_properties()
    {
        await using var db = await SqliteTestDatabase.CreateAsync();
        var first = EntityFactory.Property("First");
        var second = EntityFactory.Property("Second");
        var unit = EntityFactory.Unit(EntityFactory.Building(second), "1");
        var bay = new ParkingBay { Property = first, ParkingBayNumber = "P1" };
        var store = new StoreRoom { Property = first, StoreRoomNumber = "S1" };
        db.Context.AddRange(unit, bay, store);
        await db.Context.SaveChangesAsync();
        var service = new PropertyAllocationService(db.Context);

        await Assert.ThrowsAsync<BusinessRuleException>(() => service.AllocateParkingBayAsync(bay.ParkingBayId, unit.UnitId));
        await Assert.ThrowsAsync<BusinessRuleException>(() => service.AllocateStoreRoomAsync(store.StoreRoomId, unit.UnitId));
    }
}
