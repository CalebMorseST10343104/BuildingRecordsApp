using System;
using System.ComponentModel.DataAnnotations;

namespace BuildingRecordsApp.Models.ItemViewModels;

public class StoreRoomItemViewModel
{
    public int? StoreRoomId { get; set; }

    [Display(Name = "Store Room Number")]
    public string? StoreRoomNumber { get; set; }

    public Dictionary<string, object?> ToDictionary()
    {
        return new Dictionary<string, object?>
        {
            { nameof(StoreRoomId), StoreRoomId },
            { nameof(StoreRoomNumber), StoreRoomNumber }
        };
    }
}
