using System;
using System.ComponentModel.DataAnnotations;
using BuildingRecordsApp.Attributes;

namespace BuildingRecordsApp.Models.ItemViewModels;

public class UnitItemViewModel : ItemViewModel
{
    public int? UnitId { get; set; }

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
}
