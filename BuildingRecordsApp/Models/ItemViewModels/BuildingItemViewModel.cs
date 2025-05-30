using System;
using System.ComponentModel.DataAnnotations;

namespace BuildingRecordsApp.Models.ItemViewModels;

public class BuildingItemViewModel
{
    public int? BuildingId { get; set; }

    [Display(Name = "Building Name")]
    public string? Name { get; set; }

    [Display(Name = "Address")]
    public string? Address { get; set; }

    [Display(Name = "Number of Units")]
    public int? NumberOfUnits { get; set; }

    [Display(Name = "Number of Floors")]
    public int? NumberOfFloors { get; set; }
    
    public Dictionary<string, object?> BuildingDetails()
    {
        return new Dictionary<string, object?>
        {
            { nameof(BuildingId), BuildingId },
            { nameof(Name), Name },
            { nameof(Address), Address },
            { nameof(NumberOfUnits), NumberOfUnits },
            { nameof(NumberOfFloors), NumberOfFloors }
        };
    }
}
