using System;
using BuildingRecordsApp.Models.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace BuildingRecordsApp.Models.FormViewModels;

public class OwnershipFormViewModel
{
    public Ownership? Ownership { get; set; }

    public SelectList UnitSelectList { get; set; } = new SelectList(Enumerable.Empty<SelectListItem>());

    public SelectList CompanyTrustSelectList { get; set; } = new SelectList(Enumerable.Empty<SelectListItem>());
}
