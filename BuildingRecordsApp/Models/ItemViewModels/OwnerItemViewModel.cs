using System;
using System.ComponentModel.DataAnnotations;

namespace BuildingRecordsApp.Models.ItemViewModels;

public class OwnerItemViewModel
{
    public int? OwnerId { get; set; }

    //Related model display fields
    [Display(Name = "Building Name")]
    public string? BuildingName { get; set; }

    [Display(Name = "Unit Number")]
    public string? UnitNumber { get; set; }

    [Display(Name = "First Name")]
    public string? FirstName { get; set; }

    [Display(Name = "Last Name")]
    public string? LastName { get; set; }
}
