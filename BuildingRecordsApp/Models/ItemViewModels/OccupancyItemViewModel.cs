using System;
using System.ComponentModel.DataAnnotations;
using BuildingRecordsApp.Attributes;

namespace BuildingRecordsApp.Models.ItemViewModels;

public class OccupancyItemViewModel : ItemViewModel
{
    public int? OccupancyId { get; set; }

    [Display(Name = "Occupation Type")]
    [DisplayMode("Basic")]
    public string? OccupationType { get; set; }
}
