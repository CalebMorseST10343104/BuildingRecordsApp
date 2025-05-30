using System;
using System.ComponentModel.DataAnnotations;

namespace BuildingRecordsApp.Models.ItemViewModels;

public class OccupancyItemViewModel
{
    public int? OccupancyId { get; set; }

    [Display(Name = "Occupation Type")]
    public string? OccupationType { get; set; }

    //Related model display fields
    [Display(Name = "Building")]
    public string? BuildingName { get; set; }

    [Display(Name = "Unit Number")]
    public string? UnitNumber { get; set; }

    [Display(Name = "First Name")]
    public string? OccupantFirstName { get; set; }

    [Display(Name = "Last Name")]
    public string? OccupantLastName { get; set; }
}
