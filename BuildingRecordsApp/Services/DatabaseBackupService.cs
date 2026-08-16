using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BuildingRecordsApp.Services;

public sealed class DatabaseBackupService : IDatabaseBackupService
{
    private readonly BuildingContext _context;
    private readonly string _backupDirectory;
    private readonly int _retainedBackupCount;

    public DatabaseBackupService(
        BuildingContext context,
        IOptions<DatabaseBackupOptions> options,
        IWebHostEnvironment environment)
    {
        _context = context;
        var configuredDirectory = options.Value.Directory;
        _backupDirectory = Path.GetFullPath(
            Path.IsPathRooted(configuredDirectory)
                ? configuredDirectory
                : Path.Combine(environment.ContentRootPath, configuredDirectory));
        _retainedBackupCount = Math.Max(1, options.Value.RetainedBackupCount);
    }

    public async Task<DatabaseBackupInfo> CreateAsync(string reason, CancellationToken cancellationToken = default)
    {
        var sourceConnectionString = _context.Database.GetConnectionString()
            ?? throw new InvalidOperationException("The database connection is not configured.");
        var sourceSettings = new SqliteConnectionStringBuilder(sourceConnectionString);
        if (string.IsNullOrWhiteSpace(sourceSettings.DataSource) || sourceSettings.DataSource == ":memory:")
            throw new InvalidOperationException("Only a file-based SQLite database can be backed up.");

        Directory.CreateDirectory(_backupDirectory);
        var safeReason = string.Concat(reason.ToLowerInvariant().Select(c => char.IsLetterOrDigit(c) ? c : '-')).Trim('-');
        if (string.IsNullOrEmpty(safeReason))
            safeReason = "manual";
        var fileName = $"building-register-{DateTime.UtcNow:yyyyMMdd-HHmmssfff}-{safeReason}-{Guid.NewGuid():N}.db";
        var destinationPath = Path.Combine(_backupDirectory, fileName);

        var integrityPassed = false;
        await using (var source = new SqliteConnection(sourceConnectionString))
        await using (var destination = new SqliteConnection($"Data Source={destinationPath}"))
        {
            await source.OpenAsync(cancellationToken);
            await destination.OpenAsync(cancellationToken);
            source.BackupDatabase(destination);

            await using var check = destination.CreateCommand();
            check.CommandText = "PRAGMA quick_check;";
            var result = await check.ExecuteScalarAsync(cancellationToken);
            integrityPassed = string.Equals(result?.ToString(), "ok", StringComparison.OrdinalIgnoreCase);
        }

        if (!integrityPassed)
        {
            File.Delete(destinationPath);
            throw new InvalidOperationException("The backup failed its database integrity check.");
        }

        PruneOldBackups();
        return ToInfo(new FileInfo(destinationPath));
    }

    public IReadOnlyList<DatabaseBackupInfo> List()
    {
        if (!Directory.Exists(_backupDirectory))
            return [];

        return new DirectoryInfo(_backupDirectory)
            .EnumerateFiles("building-register-*.db")
            .OrderByDescending(file => file.CreationTimeUtc)
            .Select(ToInfo)
            .ToList();
    }

    public string? ResolveExistingBackup(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName) || fileName != Path.GetFileName(fileName))
            return null;
        var candidate = Path.GetFullPath(Path.Combine(_backupDirectory, fileName));
        return candidate.StartsWith(_backupDirectory + Path.DirectorySeparatorChar, StringComparison.Ordinal)
            && File.Exists(candidate) ? candidate : null;
    }

    private void PruneOldBackups()
    {
        foreach (var file in new DirectoryInfo(_backupDirectory)
                     .EnumerateFiles("building-register-*.db")
                     .OrderByDescending(file => file.CreationTimeUtc)
                     .Skip(_retainedBackupCount))
            file.Delete();
    }

    private static DatabaseBackupInfo ToInfo(FileInfo file) =>
        new(file.Name, file.CreationTimeUtc, file.Length);
}
