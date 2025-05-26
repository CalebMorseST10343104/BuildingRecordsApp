using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using BuildingRecordsApp.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace BuildingRecordsApp.Models.FormViewModels;

public class OwnerFormViewModel
{
    public int? OwnerId { get; set; }

    public int? PersonId { get; set; } // Foreign key for Person
    public int? OwnershipId { get; set; } // Foreign key for Ownership
    
    public SelectList OwnershipSelectList { get; set; } = new SelectList(Enumerable.Empty<SelectListItem>());
    public SelectList PersonSelectList { get; set; } = new SelectList(Enumerable.Empty<SelectListItem>());
    

}
