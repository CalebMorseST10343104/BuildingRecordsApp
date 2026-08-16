namespace BuildingRecordsApp.Services;

public sealed record DatabaseBackupInfo(string FileName, DateTime CreatedAtUtc, long SizeBytes);
