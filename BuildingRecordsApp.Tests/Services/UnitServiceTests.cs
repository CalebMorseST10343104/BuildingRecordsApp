using BuildingRecordsApp.Services;
using BuildingRecordsApp.Tests.Database;
using Microsoft.EntityFrameworkCore;

namespace BuildingRecordsApp.Tests.Services;

public class UnitServiceTests
{
    [Fact]
    public async Task Create_adds_unit_and_exactly_one_access_count_record()
    {
        await using var db = await SqliteTestDatabase.CreateAsync();
        var building = EntityFactory.Building(EntityFactory.Property());
        db.Context.Buildings.Add(building);
        await db.Context.SaveChangesAsync();

        var unit = await new UnitService(db.Context).CreateAsync(EntityFactory.Unit(building, " 101 "));

        Assert.Equal("101", unit.UnitNumber);
        Assert.Equal(1, await db.Context.AccessDeviceCounts.CountAsync(t => t.UnitId == unit.UnitId));
        Assert.Null(unit.AccessDeviceCount!.OwnershipContactTagCount);
    }

    [Fact]
    public async Task Create_rejects_a_unit_without_a_building()
    {
        await using var db = await SqliteTestDatabase.CreateAsync();
        await Assert.ThrowsAsync<BusinessRuleException>(() => new UnitService(db.Context).CreateAsync(new() { UnitNumber = "101" }));
    }
}
