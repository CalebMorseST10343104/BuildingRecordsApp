using System;
using BuildingRecordsApp.Enums;
using BuildingRecordsApp.Models.Entities;

namespace BuildingRecordsApp.Models.DisplayViewModels;

public class UnitDisplayViewModel
{
    public List<Unit> Units { get; set; } = [];
    public List<int> IdsToDisplay { get; set; } = [];
    public DisplayMode DisplayMode { get; set; } = DisplayMode.Basic;
    public DisplayLayout DisplayLayout { get; set; } = DisplayLayout.List;
}
