using System;
using System.ComponentModel.DataAnnotations;
using BuildingRecordsApp.Attributes;

namespace BuildingRecordsApp.Models.ItemViewModels;

public class OccupancyItemViewModel : ItemViewModel, IItemViewModel
{
    [DisplayMode("Full")]
    public int? OccupancyId { get; set; }

    [Display(Name = "Occupant First Name")]
    [DisplayMode("Extended")]
    public string? OccupantFirstName { get; set; }

    [Display(Name = "Occupant Last Name")]
    [DisplayMode("Extended")]
    public string? OccupantLastName { get; set; }

    [Display(Name = "Building Name")]
    [DisplayMode("Extended")]
    public string? BuildingName { get; set; }

    [Display(Name = "Unit Number")]
    [DisplayMode("Extended")]
    public string? UnitNumber { get; set; }

    [Display(Name = "Occupation Type")]
    [DisplayMode("Basic")]
    public string? OccupationType { get; set; }

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
