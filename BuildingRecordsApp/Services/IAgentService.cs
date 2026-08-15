using BuildingRecordsApp.Models.Entities;

namespace BuildingRecordsApp.Services;

public interface IAgentService
{
    Task<Agent> CreateProfileAsync(int personId, int agentCompanyId, CancellationToken cancellationToken = default);
    Task AssignToUnitAsync(int unitId, int? agentId, CancellationToken cancellationToken = default);
}
