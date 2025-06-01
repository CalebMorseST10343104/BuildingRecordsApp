using System;
using System.ComponentModel.DataAnnotations;
using BuildingRecordsApp.Attributes;

namespace BuildingRecordsApp.Models.ItemViewModels;

public class UnitItemViewModel : ItemViewModel
{
    [DisplayMode("Full")]
    public int? UnitId { get; set; }

    [Display(Name = "Building Name")]
    [DisplayMode("Extended")]
    public string? BuildingName { get; set; }

    [Display(Name = "Unit Number")]
    [DisplayMode("Basic")]
    public string? UnitNumber { get; set; }

    [Display(Name = "Bedroom Count")]
    [DisplayMode("Detailed")]
    public int? Bedrooms { get; set; }

    [Display(Name = "Has DB Inverter?")]
    [DisplayMode("Full")]
    public bool? DbInverter { get; set; }

    [Display(Name = "Has Housekeeping?")]
    [DisplayMode("Full")]
    public bool? Housekeeping { get; set; }

    [Display(Name = "Is Pet Friendly?")]
    [DisplayMode("Full")]
    public bool? PetFriendly { get; set; }

    [Display(Name = "Allows Subletting?")]
    [DisplayMode("Detailed")]
    public bool? SublettingAllowed { get; set; }

    [Display(Name = "AC Unit Count")]
    [DisplayMode("Full")]
    public int? AirconditioningUnits { get; set; }

    public override int GetId()
    {
        return UnitId ?? 0;
    }

    public override string GetTitleHeader()
    {
        if (string.IsNullOrEmpty(BuildingName) && string.IsNullOrEmpty(UnitNumber))
        {
            return "Unit Details";
        }
        else
        {
            return $"{BuildingName} - Unit {UnitNumber}".Trim();
        }
    }

    public override bool IsTitleHeaderFieldName(object item)
    {
        return item is nameof(BuildingName) || item is nameof(UnitNumber);
    }
}
