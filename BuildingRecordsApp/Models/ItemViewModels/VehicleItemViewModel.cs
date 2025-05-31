using System;
using System.ComponentModel.DataAnnotations;
using BuildingRecordsApp.Attributes;

namespace BuildingRecordsApp.Models.ItemViewModels;

public class VehicleItemViewModel : ItemViewModel, IItemViewModel
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

    public string GetTitleHeader(string valueIfNull)
    {
        throw new NotImplementedException();
    }

    public string GetTitleHeaderFieldName(bool formatted = false)
    {
        throw new NotImplementedException();
    }

    public bool HasTitleHeader()
    {
        throw new NotImplementedException();
    }
}
