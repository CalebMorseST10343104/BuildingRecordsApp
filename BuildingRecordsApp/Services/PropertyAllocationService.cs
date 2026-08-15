using Microsoft.EntityFrameworkCore;

namespace BuildingRecordsApp.Services;

public sealed class PropertyAllocationService(BuildingContext context) : IPropertyAllocationService
{
    public async Task AllocateParkingBayAsync(int parkingBayId, int? unitId, CancellationToken cancellationToken = default)
    {
        var bay = await context.ParkingBays.SingleOrDefaultAsync(p => p.ParkingBayId == parkingBayId, cancellationToken)
            ?? throw new BusinessRuleException("Parking bay not found.");
        await EnsureSamePropertyAsync(bay.PropertyId, unitId, cancellationToken);
        bay.UnitID = unitId;
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task AllocateStoreRoomAsync(int storeRoomId, int? unitId, CancellationToken cancellationToken = default)
    {
        var storeRoom = await context.StoreRooms.SingleOrDefaultAsync(s => s.StoreRoomId == storeRoomId, cancellationToken)
            ?? throw new BusinessRuleException("Storeroom not found.");
        await EnsureSamePropertyAsync(storeRoom.PropertyId, unitId, cancellationToken);
        storeRoom.UnitId = unitId;
        await context.SaveChangesAsync(cancellationToken);
    }

    private async Task EnsureSamePropertyAsync(int propertyId, int? unitId, CancellationToken cancellationToken)
    {
        if (unitId is null)
            return;

        var unitPropertyId = await context.Units
            .Where(u => u.UnitId == unitId)
            .Select(u => (int?)u.Building!.PropertyId)
            .SingleOrDefaultAsync(cancellationToken)
            ?? throw new BusinessRuleException("Unit not found.");

        if (unitPropertyId != propertyId)
            throw new BusinessRuleException("The allocation must remain within the same property.");
    }
}
