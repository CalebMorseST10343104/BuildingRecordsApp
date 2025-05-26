using System;
using BuildingRecordsApp.Models.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;


namespace BuildingRecordsApp.Models.FormViewModels;

public class OwnershipFormViewModel
{
    public int? OwnershipId { get; set; }

    [Display(Name = "Ownership Type")]
    public string? OwnershipType { get; set; } // e.g., "Natural", "Juristic"

    public int? UnitId { get; set; } // Foreign key
    public int? CompanyTrustId { get; set; } // Foreign key for CompanyTrust

    public SelectList UnitSelectList { get; set; } = new SelectList(Enumerable.Empty<SelectListItem>());

    public SelectList CompanyTrustSelectList { get; set; } = new SelectList(Enumerable.Empty<SelectListItem>());
}
