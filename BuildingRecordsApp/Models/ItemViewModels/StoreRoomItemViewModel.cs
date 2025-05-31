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

    public override string GetTitleHeader()
    {
        if (string.IsNullOrEmpty(StoreRoomNumber))
        {
            return "Store Room";
        }
        else
        {
            return $"Store Room {StoreRoomNumber}";
        }
    }

    public override bool IsTitleHeaderFieldName(object item)
    {
        return item is nameof(StoreRoomNumber);
    }
}
