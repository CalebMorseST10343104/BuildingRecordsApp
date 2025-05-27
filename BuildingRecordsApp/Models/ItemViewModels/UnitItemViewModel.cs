using System;
using System.ComponentModel.DataAnnotations;

namespace BuildingRecordsApp.Models.ItemViewModels;

public class UnitItemViewModel
{
    public int? UnitId { get; set; }

    [Display(Name = "Unit Number")]
    public string? UnitNumber { get; set; }

    [Display(Name = "Bedroom Count")]
    public int? Bedrooms { get; set; }

    [Display(Name = "Has DB Inverter?")]
    public bool? DbInverter { get; set; }

    [Display(Name = "Has Housekeeping?")]
    public bool? Housekeeping { get; set; }

    [Display(Name = "Is Pet Friendly?")]
    public bool? PetFriendly { get; set; }

    [Display(Name = "Allows Subletting?")]
    public bool? SublettingAllowed { get; set; }

    [Display(Name = "AC Unit Count")]
    public int? AirconditioningUnits { get; set; }
}
