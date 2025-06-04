using System;
using BuildingRecordsApp.Enums;

namespace BuildingRecordsApp.Models.DisplayViewModels;

public class DisplayViewModel<TItem> : IDisplayViewModel<TItem>
    where TItem : ItemViewModels.IItemViewEntry
{
    public List<TItem> Entries { get; set; } = [];
    public List<int> IdsToDisplay { get; set; } = [];
    public DisplayMode DisplayMode { get; set; } = DisplayMode.Basic;
    public DisplayLayout DisplayLayout { get; set; } = DisplayLayout.List;
    public bool ShowActions { get; set; } = false;
}
