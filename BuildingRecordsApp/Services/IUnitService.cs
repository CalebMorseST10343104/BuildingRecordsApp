using BuildingRecordsApp.Models.Entities;

namespace BuildingRecordsApp.Services;

public interface IUnitService
{
    Task<Unit> CreateAsync(Unit unit, CancellationToken cancellationToken = default);
}
