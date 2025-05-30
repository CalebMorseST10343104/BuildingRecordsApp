using System;
using System.ComponentModel.DataAnnotations;

namespace BuildingRecordsApp.Models.ItemViewModels;

public class VehicleItemViewModel
{
    public int? VehicleId { get; set; }

    [Display(Name = "Vehicle Registration")]
    public string? VehicleRegistration { get; set; }

    [Display(Name = "Vehicle Model")]
    public string? VehicleModel { get; set; }

    [Display(Name = "Vehicle Make")]
    public string? VehicleMake { get; set; }

    [Display(Name = "Vehicle Colour")]
    public string? VehicleColor { get; set; }

    // Related model display fields
    [Display(Name = "Building Name")]
    public string? BuildingName { get; set; }

    [Display(Name = "Unit Number")]
    public string? UnitNumber { get; set; }
}
