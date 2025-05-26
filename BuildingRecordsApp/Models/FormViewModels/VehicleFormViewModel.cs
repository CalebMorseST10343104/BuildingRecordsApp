using System;
using BuildingRecordsApp.Models.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BuildingRecordsApp.Models.FormViewModels;

public class VehicleFormViewModel
{
    public int? VehicleId { get; set; }

    [Display(Name = "Vehicle Registration")]
    public string? VehicleRegistration { get; set; }

    [Display(Name = "Vehicle Model")]
    public string? VehicleModel { get; set; }

    [Display(Name = "Vehicle Make")]
    public string? VehicleMake { get; set; }

    [Display(Name = "Vehicle Colour")]
    public string? VehicleColor { get; set; }

    public int? UnitId { get; set; } // Foreign key to Unit

    public SelectList UnitSelectList { get; set; } = new SelectList(Enumerable.Empty<SelectListItem>());
}
