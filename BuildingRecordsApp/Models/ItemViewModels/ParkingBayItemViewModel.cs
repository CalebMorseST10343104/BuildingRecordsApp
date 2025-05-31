using System;
using System.ComponentModel.DataAnnotations;
using BuildingRecordsApp.Attributes;

namespace BuildingRecordsApp.Models.ItemViewModels;

public class ParkingBayItemViewModel : ItemViewModel, IItemViewModel
{
    [DisplayMode("Full")]
    public int? ParkingBayId { get; set; }

    [Display(Name = "Parking Bay Number")]
    [DisplayMode("Basic")]
    public string? ParkingBayNumber { get; set; }

    [Display(Name = "Is Near Entrance")]
    [DisplayMode("Detailed")]
    public bool? IsNearEntrance { get; set; }

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
