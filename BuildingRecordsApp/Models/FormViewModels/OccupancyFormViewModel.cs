using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using BuildingRecordsApp.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace BuildingRecordsApp.Models.FormViewModels;

public class OccupancyFormViewModel
{
    public int? OccupancyId { get; set; }

    [Display(Name = "Occupation Type")]
    public string? OccupationType { get; set; } // e.g., "Owner", "Short-Term Rental", "Long-Term Rental"
    
    public int? UnitId { get; set; } // Foreign key for Unit
    public int? OccupantId { get; set; } // Foreign key for Person
    public SelectList UnitSelectList { get; set; } = new SelectList(Enumerable.Empty<SelectListItem>());
    public SelectList PersonSelectList { get; set; } = new SelectList(Enumerable.Empty<SelectListItem>());
}
