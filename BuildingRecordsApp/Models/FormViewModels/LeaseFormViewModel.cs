using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using BuildingRecordsApp.Models.Entities;

namespace BuildingRecordsApp.Models.FormViewModels;

public class LeaseFormViewModel
{
    public Lease? Lease { get; set; }
    public SelectList UnitSelectList { get; set; } = new SelectList(Enumerable.Empty<SelectListItem>());
}
