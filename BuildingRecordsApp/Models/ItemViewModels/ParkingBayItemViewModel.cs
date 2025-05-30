using System;
using System.ComponentModel.DataAnnotations;

namespace BuildingRecordsApp.Models.ItemViewModels;

public class ParkingBayItemViewModel
{
    public int? ParkingBayId { get; set; }

    [Display(Name = "Parking Bay Number")]
    public string? ParkingBayNumber { get; set; }

    [Display(Name = "Is Near Entrance")]
    public bool? IsNearEntrance { get; set; }

    // Related model display fields
    [Display(Name = "Building Name")]
    public string? BuildingName { get; set; }

    [Display(Name = "Unit Number")]
    public string? UnitNumber { get; set; }
}
