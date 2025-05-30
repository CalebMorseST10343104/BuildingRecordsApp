using System;
using System.ComponentModel.DataAnnotations;

namespace BuildingRecordsApp.Models.ItemViewModels;

public class OwnershipItemViewModel
{
    public int? OwnershipId { get; set; }

    [Display(Name = "Ownership Type")]
    public string? OwnershipType { get; set; }

    //Related model display fields
    [Display(Name = "Building Name")]
    public string? BuildingName { get; set; }

    [Display(Name = "Unit Number")]
    public string? UnitNumber { get; set; }

    [Display(Name = "Company Name")]
    public string? CompanyName { get; set; }
}
