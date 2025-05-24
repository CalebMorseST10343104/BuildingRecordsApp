using System;
using BuildingRecordsApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BuildingRecordsApp.ViewModels;

public class StoreRoomFormViewModel
{
    public StoreRoom? StoreRoom { get; set; }
    public SelectList UnitSelectList { get; set; } = new SelectList(Enumerable.Empty<SelectListItem>());
}
