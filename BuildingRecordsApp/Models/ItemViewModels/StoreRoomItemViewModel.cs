using System;
using System.ComponentModel.DataAnnotations;
using BuildingRecordsApp.Attributes;

namespace BuildingRecordsApp.Models.ItemViewModels;

public class StoreRoomItemViewModel : ItemViewModel
{
    [DisplayMode("Full")]
    public int? StoreRoomId { get; set; }

    [Display(Name = "Store Room Number")]
    [DisplayMode("Basic")]
    public string? StoreRoomNumber { get; set; }

    [Display(Name = "Building Name")]
    [DisplayMode("Extended")]
    public string? BuildingName { get; set; }

    [Display(Name = "Unit Number")]
    [DisplayMode("Extended")]
    public int? UnitNumber { get; set; }
}
