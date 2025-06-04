using System;
using BuildingRecordsApp.Enums;

namespace BuildingRecordsApp.Models.DisplayViewModels;

public interface IDisplayViewModel<TItem>
    where TItem : ItemViewModels.IItemViewEntry
{
    List<TItem> Entries { get; set; }
    List<int> IdsToDisplay { get; set; }
    DisplayMode DisplayMode { get; set; }
    DisplayLayout DisplayLayout { get; set; }
    bool ShowActions { get; set; }
}
