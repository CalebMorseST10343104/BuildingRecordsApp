using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using BuildingRecordsApp.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace BuildingRecordsApp.Models.FormViewModels;

public class OwnershipContactFormViewModel
{
    public int? OwnershipContactId { get; set; }

    [Required, Display(Name = "Person")]
    public int? PersonId { get; set; } // Foreign key for Person
    [Required, Display(Name = "Ownership")]
    public int? OwnershipId { get; set; } // Foreign key for Ownership
    
    public SelectList OwnershipSelectList { get; set; } = new SelectList(Enumerable.Empty<SelectListItem>());
    public SelectList PersonSelectList { get; set; } = new SelectList(Enumerable.Empty<SelectListItem>());
    

}
