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

    public Dictionary<string, object?> ParkingBayDetails()
    {
        return new Dictionary<string, object?>
        {
            { nameof(ParkingBayId), ParkingBayId },
            { nameof(ParkingBayNumber), ParkingBayNumber },
            { nameof(IsNearEntrance), IsNearEntrance }
        };
    }
}
