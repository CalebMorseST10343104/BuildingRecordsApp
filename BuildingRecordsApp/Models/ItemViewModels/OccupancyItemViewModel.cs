using System;
using System.ComponentModel.DataAnnotations;

namespace BuildingRecordsApp.Models.ItemViewModels;

public class OccupancyItemViewModel
{
    public int? OccupancyId { get; set; }

    [Display(Name = "Occupation Type")]
    public string? OccupationType { get; set; }

    public Dictionary<string, object?> ToDictionary()
    {
        return new Dictionary<string, object?>
        {
            { nameof(OccupancyId), OccupancyId },
            { nameof(OccupationType), OccupationType }
        };
    }
}
