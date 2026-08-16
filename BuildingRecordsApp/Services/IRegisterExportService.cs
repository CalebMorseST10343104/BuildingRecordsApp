namespace BuildingRecordsApp.Services;

public interface IRegisterExportService
{
    Task<RegisterExportResult> ExportExcelAsync(
        int propertyId,
        IReadOnlyCollection<int> buildingIds,
        CancellationToken cancellationToken = default);
}
