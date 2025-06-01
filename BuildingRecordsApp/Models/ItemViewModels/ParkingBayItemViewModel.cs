using System;
using System.ComponentModel.DataAnnotations;
using BuildingRecordsApp.Attributes;

namespace BuildingRecordsApp.Models.ItemViewModels;

public class ParkingBayItemViewModel : ItemViewModel
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

    public override int GetId()
    {
        return ParkingBayId ?? 0;
    }

    public override string GetTitleHeader()
    {
        if (string.IsNullOrEmpty(ParkingBayNumber))
        {
            return "Parking Bay";
        }
        else
        {
            return $"Parking Bay {ParkingBayNumber}";
        }
    }

    public override bool IsTitleHeaderFieldName(object item)
    {
        return item is nameof(ParkingBayNumber);
    }
}
