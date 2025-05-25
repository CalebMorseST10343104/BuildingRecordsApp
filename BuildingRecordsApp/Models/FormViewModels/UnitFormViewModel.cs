using System;
using BuildingRecordsApp.Models.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BuildingRecordsApp.Models.FormViewModels;

public class UnitFormViewModel
{
    public Unit? Unit { get; set; }
    public SelectList BuildingSelectList { get; set; } = new SelectList(Enumerable.Empty<SelectListItem>());
}
