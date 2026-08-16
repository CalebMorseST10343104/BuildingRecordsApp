namespace BuildingRecordsApp.Services;

public interface IDatabaseBackupService
{
    Task<DatabaseBackupInfo> CreateAsync(string reason, CancellationToken cancellationToken = default);
    IReadOnlyList<DatabaseBackupInfo> List();
    string? ResolveExistingBackup(string fileName);
}
