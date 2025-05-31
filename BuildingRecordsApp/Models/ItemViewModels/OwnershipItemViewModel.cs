using System;
using System.ComponentModel.DataAnnotations;
using BuildingRecordsApp.Attributes;

namespace BuildingRecordsApp.Models.ItemViewModels;

public class OwnershipItemViewModel : ItemViewModel
{
    [DisplayMode("Full")]
    public int? OwnershipId { get; set; }

    [Display(Name = "Building Name")]
    [DisplayMode("Extended")]
    public string? BuildingName { get; set; }

    [Display(Name = "Unit Number")]
    [DisplayMode("Extended")]
    public int? UnitNumber { get; set; }

    [Display(Name = "Company Name")]
    [DisplayMode("Extended")]
    public string? CompanyName { get; set; }

    [Display(Name = "Ownership Type")]
    [DisplayMode("Basic")]
    public string? OwnershipType { get; set; }
}
