using System;
using BuildingRecordsApp.Enums;

namespace BuildingRecordsApp.Models.DisplayViewModels;

public interface IDisplayViewModel
{
    List<int> IdsToDisplay { get; set; }
    DisplayMode DisplayMode { get; set; }
    DisplayLayout DisplayLayout { get; set; }
}
