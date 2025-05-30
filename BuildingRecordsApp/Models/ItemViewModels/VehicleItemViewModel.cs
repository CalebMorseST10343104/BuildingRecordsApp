using System;
using System.ComponentModel.DataAnnotations;
using BuildingRecordsApp.Attributes;

namespace BuildingRecordsApp.Models.ItemViewModels;

public class VehicleItemViewModel : ItemViewModel
{
    public int? VehicleId { get; set; }

    [Display(Name = "Vehicle Registration")]
    [DisplayMode("Basic")]
    public string? VehicleRegistration { get; set; }

    [Display(Name = "Vehicle Model")]
    [DisplayMode("Detailed")]
    public string? VehicleModel { get; set; }

    [Display(Name = "Vehicle Make")]
    [DisplayMode("Detailed")]
    public string? VehicleMake { get; set; }

    [Display(Name = "Vehicle Colour")]
    [DisplayMode("Detailed")]
    public string? VehicleColor { get; set; }
}
