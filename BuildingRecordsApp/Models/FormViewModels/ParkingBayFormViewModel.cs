using System;
using BuildingRecordsApp.Models.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BuildingRecordsApp.Models.FormViewModels;

public class ParkingBayFormViewModel
{
    public int? ParkingBayId { get; set; }
    public int PropertyId { get; set; }

    [Display(Name = "Parking Bay Number")]
    public string? ParkingBayNumber { get; set; }

    [Display(Name = "Is Near Entrance")]
    public bool IsNearEntrance { get; set; }


    public int? UnitID { get; set; } // Foreign key

    public SelectList UnitSelectList { get; set; } = new SelectList(Enumerable.Empty<SelectListItem>());
    
}
