namespace BuildingRecordsApp.Models;

public static class OccupancyTypes
{
    public static readonly IReadOnlyList<string> All =
    [
        "Owner",
        "Owner & Long-Term Letting",
        "Owner & Short-Term Letting",
        "Owner Family",
        "Short-Term Letting",
        "Tenant",
        "Tenant & Short-Term Letting"
    ];
}
