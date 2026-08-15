using BuildingRecordsApp.Models.Entities;

namespace BuildingRecordsApp.Services;

public interface IOwnershipService
{
    Task<Ownership> SetOwnershipAsync(int unitId, string ownershipType, int? organizationId, CancellationToken cancellationToken = default);
    Task AddContactAsync(int ownershipId, int personId, CancellationToken cancellationToken = default);
    Task RemoveContactAsync(int ownershipId, int personId, CancellationToken cancellationToken = default);
}
