using System;
using BuildingRecordsApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BuildingRecordsApp.ViewModels;

public class VehicleFormViewModel
{
    public Vehicle? Vehicle { get; set; }
    public SelectList UnitSelectList { get; set; } = new SelectList(Enumerable.Empty<SelectListItem>());
}
