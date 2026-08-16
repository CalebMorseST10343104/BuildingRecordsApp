namespace BuildingRecordsApp.Services;

public sealed class DatabaseBackupOptions
{
    public string Directory { get; set; } = "Backups";
    public int RetainedBackupCount { get; set; } = 30;
}
