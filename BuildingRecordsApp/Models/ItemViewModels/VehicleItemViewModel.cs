using System;
using System.ComponentModel.DataAnnotations;
using BuildingRecordsApp.Attributes;

namespace BuildingRecordsApp.Models.ItemViewModels;

public class VehicleItemViewModel : ItemViewModel
{
    [DisplayMode("Full")]
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

    [Display(Name = "Building Name")]
    [DisplayMode("Extended")]
    public string? BuildingName { get; set; }

    [Display(Name = "Unit Number")]
    [DisplayMode("Extended")]
    public int? UnitNumber { get; set; }

    public override string GetTitleHeader()
    {
        if (string.IsNullOrEmpty(VehicleRegistration))
        {
            return "Vehicle Details";
        }
        else
        {
            return $"Vehicle {VehicleRegistration}";
        }
    }

    public override bool IsTitleHeaderFieldName(object item)
    {
        return item is nameof(VehicleRegistration);
    }
}
