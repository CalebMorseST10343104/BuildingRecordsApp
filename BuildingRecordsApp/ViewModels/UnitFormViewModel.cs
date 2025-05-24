using System;
using BuildingRecordsApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BuildingRecordsApp.ViewModels;

public class UnitFormViewModel
{
    public Unit? Unit { get; set; }
    public SelectList BuildingSelectList { get; set; } = new SelectList(Enumerable.Empty<SelectListItem>());
}
