using System;
using BuildingRecordsApp.Models.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;


namespace BuildingRecordsApp.Models.FormViewModels;

public class OwnershipFormViewModel : IValidatableObject
{
    public int? OwnershipId { get; set; }

    [Required, Display(Name = "Ownership Type")]
    public string? OwnershipType { get; set; } // e.g., "Natural", "Juristic"

    [Required, Display(Name = "Unit")]
    public int? UnitId { get; set; } // Foreign key
    public int? OrganizationId { get; set; } // Foreign key for Organization

    public SelectList UnitSelectList { get; set; } = new SelectList(Enumerable.Empty<SelectListItem>());

    public SelectList OrganizationSelectList { get; set; } = new SelectList(Enumerable.Empty<SelectListItem>());

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (OwnershipType == "Juristic" && OrganizationId is null)
            yield return new ValidationResult(
                "Select a company or trust for juristic ownership.",
                [nameof(OrganizationId)]);
        if (OwnershipType == "Natural" && OrganizationId is not null)
            yield return new ValidationResult(
                "A company or trust cannot be selected for natural ownership.",
                [nameof(OrganizationId)]);
    }
}
