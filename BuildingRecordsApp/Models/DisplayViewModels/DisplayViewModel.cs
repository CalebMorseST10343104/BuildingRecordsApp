using System;
using BuildingRecordsApp.Enums;

namespace BuildingRecordsApp.Models.DisplayViewModels;

public class DisplayViewModel<TItem> : IDisplayViewModel
    where TItem : ItemViewModels.IDisplayEntry
{
    public List<TItem> Entries { get; set; } = [];
    public List<int> IdsToDisplay { get; set; } = [];
    public DisplayMode DisplayMode { get; set; } = DisplayMode.Basic;
    public DisplayLayout DisplayLayout { get; set; } = DisplayLayout.List;
    public bool ShowActions { get; set; } = false;
}
