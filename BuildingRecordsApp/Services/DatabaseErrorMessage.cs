namespace BuildingRecordsApp.Services;

public sealed record DatabaseErrorMessage(DatabaseErrorKind Kind, string UserMessage);
