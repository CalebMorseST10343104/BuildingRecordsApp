using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using BuildingRecordsApp.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace BuildingRecordsApp.Models.FormViewModels;

public class LeaseFormViewModel : IValidatableObject
{
    public int? LeaseId { get; set; }

    [Display(Name = "Lease Holder Name")]
    public string? LeaseHolderName { get; set; }

    [DataType(DataType.Date), Display(Name = "Start Date")]
    public DateTime? StartDate { get; set; }

    [DataType(DataType.Date), Display(Name = "End Date")]
    public DateTime? EndDate { get; set; }

    [Range(0, int.MaxValue), Display(Name = "Number of Occupants")]
    public int? DeclaredOccupantCount { get; set; }

    [Display(Name = "Signed Conduct Rules?")]
    public bool SignedRules { get; set; }

    [Display(Name = "Allowed Pets?")]
    public bool PetsPresent { get; set; }

    [Display(Name = "Emergency Contact Number")]
    public string? EmergencyContactNumber { get; set; }

    public int? UnitId { get; set; } // Foreign key to Unit
    public SelectList UnitSelectList { get; set; } = new SelectList(Enumerable.Empty<SelectListItem>());

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (StartDate.HasValue && EndDate.HasValue && EndDate < StartDate)
            yield return new ValidationResult("The lease end date cannot be before its start date.", [nameof(EndDate)]);
    }
}
