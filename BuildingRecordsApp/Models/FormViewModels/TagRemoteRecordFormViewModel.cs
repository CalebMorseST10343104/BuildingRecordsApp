using System;
using BuildingRecordsApp.Models.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BuildingRecordsApp.Models.FormViewModels;

public class TagRemoteRecordFormViewModel
{
    public TagRemoteRecord? TagRemoteRecord { get; set; }
    public SelectList UnitSelectList { get; set; } = new SelectList(Enumerable.Empty<SelectListItem>());
}
