namespace BuildingRecordsApp.Services;

public interface IDatabaseErrorTranslator
{
    DatabaseErrorMessage Translate(Exception exception);
}
