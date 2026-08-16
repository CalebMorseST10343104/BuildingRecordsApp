namespace BuildingRecordsApp.Services;

public sealed record CompletenessIssue(
    string Code,
    CompletenessSeverity Severity,
    RegisterRecordType RecordType,
    int RecordId,
    string RecordLabel,
    string Summary,
    string ActionUrl,
    int? PropertyId = null,
    string? PropertyName = null,
    int? BuildingId = null,
    string? BuildingName = null,
    int? UnitId = null,
    string? UnitNumber = null);
