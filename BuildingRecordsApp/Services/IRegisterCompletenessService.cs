namespace BuildingRecordsApp.Services;

public interface IRegisterCompletenessService
{
    Task<IReadOnlyList<CompletenessIssue>> GetIssuesAsync(CancellationToken cancellationToken = default);
}
