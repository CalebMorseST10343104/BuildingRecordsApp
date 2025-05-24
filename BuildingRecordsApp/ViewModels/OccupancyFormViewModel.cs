using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using BuildingRecordsApp.Models;

namespace BuildingRecordsApp.ViewModels;

public class OccupancyFormViewModel
{
    public Occupancy? Occupancy { get; set; }
    public SelectList UnitSelectList { get; set; } = new SelectList(Enumerable.Empty<SelectListItem>());
    public SelectList PersonSelectList { get; set; } = new SelectList(Enumerable.Empty<SelectListItem>());
}
