using System;
using BuildingRecordsApp.Enums;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.ItemViewModels;

namespace BuildingRecordsApp.Models.DisplayViewModels;

public class PersonDisplayViewModel
{
    public List<PersonItemViewModel> People { get; set; } = [];
    public List<int> IdsToDisplay { get; set; } = [];
    public DisplayMode DisplayMode { get; set; } = DisplayMode.Basic;
    public DisplayLayout DisplayLayout { get; set; } = DisplayLayout.List;
}
