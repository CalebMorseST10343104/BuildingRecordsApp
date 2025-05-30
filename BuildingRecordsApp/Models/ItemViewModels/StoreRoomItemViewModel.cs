using System;
using System.ComponentModel.DataAnnotations;
using BuildingRecordsApp.Attributes;

namespace BuildingRecordsApp.Models.ItemViewModels;

public class StoreRoomItemViewModel : ItemViewModel
{
    public int? StoreRoomId { get; set; }

    [Display(Name = "Store Room Number")]
    [DisplayMode("Basic")]
    public string? StoreRoomNumber { get; set; }
}
