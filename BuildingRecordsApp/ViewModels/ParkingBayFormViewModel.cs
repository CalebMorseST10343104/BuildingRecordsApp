using System;
using BuildingRecordsApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BuildingRecordsApp.ViewModels;

public class ParkingBayFormViewModel
{
    public ParkingBay? ParkingBay { get; set; }
    public SelectList UnitSelectList { get; set; } = new SelectList(Enumerable.Empty<SelectListItem>());
    
}
