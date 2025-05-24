using System;
using BuildingRecordsApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BuildingRecordsApp.ViewModels;

public class TagRemoteRecordFormViewModel
{
    public TagRemoteRecord? TagRemoteRecord { get; set; }
    public SelectList UnitSelectList { get; set; } = new SelectList(Enumerable.Empty<SelectListItem>());
}
