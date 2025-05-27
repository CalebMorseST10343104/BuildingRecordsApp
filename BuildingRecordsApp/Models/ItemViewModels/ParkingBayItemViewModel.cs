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
}
