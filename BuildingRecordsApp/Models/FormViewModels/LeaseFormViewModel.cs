using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using BuildingRecordsApp.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace BuildingRecordsApp.Models.FormViewModels;

public class LeaseFormViewModel
{
    public int? LeaseId { get; set; }

    [Display(Name = "Lease Holder Name")]
    public string? LeaseHolderName { get; set; }

    [Display(Name = "Start Date")]
    public DateTime? StartDate { get; set; }

    [Display(Name = "End Date")]
    public DateTime? EndDate { get; set; }

    [Display(Name = "Number of Occupants")]
    public int? PersonsOccupying { get; set; }

    [Display(Name = "Signed Conduct Rules?")]
    public bool? SignedRules { get; set; }

    [Display(Name = "Allowed Pets?")]
    public bool? AllowedPets { get; set; }

    [Display(Name = "Emergency Contact Number")]
    public string? EmergencyContactNumber { get; set; }

    public int? UnitId { get; set; } // Foreign key to Unit
    public SelectList UnitSelectList { get; set; } = new SelectList(Enumerable.Empty<SelectListItem>());
}
