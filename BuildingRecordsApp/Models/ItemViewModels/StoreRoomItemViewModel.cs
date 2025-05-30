using System;
using System.ComponentModel.DataAnnotations;

namespace BuildingRecordsApp.Models.ItemViewModels;

public class StoreRoomItemViewModel
{
    public int? StoreRoomId { get; set; }

    [Display(Name = "Store Room Number")]
    public string? StoreRoomNumber { get; set; }

    // Related model display fields
    [Display(Name = "Building Name")]
    public string? BuildingName { get; set; }

    [Display(Name = "Unit Number")]
    public string? UnitNumber { get; set; }
}
