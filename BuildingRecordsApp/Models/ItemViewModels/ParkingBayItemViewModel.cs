using System;
using System.ComponentModel.DataAnnotations;
using BuildingRecordsApp.Attributes;

namespace BuildingRecordsApp.Models.ItemViewModels;

public class ParkingBayItemViewModel : ItemViewModel
{
    public int? ParkingBayId { get; set; }

    [Display(Name = "Parking Bay Number")]
    [DisplayMode("Basic")]
    public string? ParkingBayNumber { get; set; }

    [Display(Name = "Is Near Entrance")]
    [DisplayMode("Detailed")]
    public bool? IsNearEntrance { get; set; }
}
