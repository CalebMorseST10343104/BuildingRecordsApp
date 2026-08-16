namespace BuildingRecordsApp.Services;

public enum DatabaseErrorKind
{
    Duplicate,
    RecordInUse,
    MissingRequiredValue,
    InvalidValue,
    ConcurrentChange,
    TemporarilyUnavailable,
    Unknown
}
