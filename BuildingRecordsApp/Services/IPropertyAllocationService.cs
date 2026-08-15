namespace BuildingRecordsApp.Services;

public interface IPropertyAllocationService
{
    Task AllocateParkingBayAsync(int parkingBayId, int? unitId, CancellationToken cancellationToken = default);
    Task AllocateStoreRoomAsync(int storeRoomId, int? unitId, CancellationToken cancellationToken = default);
}
