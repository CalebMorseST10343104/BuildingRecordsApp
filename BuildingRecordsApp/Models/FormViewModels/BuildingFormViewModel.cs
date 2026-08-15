using System;
using BuildingRecordsApp.Models.Entities;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BuildingRecordsApp.Models.FormViewModels;

public class BuildingFormViewModel
{
    public int? BuildingId { get; set; }
    public int PropertyId { get; set; }
    public SelectList PropertySelectList { get; set; } = new SelectList(Enumerable.Empty<SelectListItem>());

    [Display(Name = "Building Name")]
    public string? Name { get; set; }

    [Display(Name = "Address")]
    public string? Address { get; set; }

    [Display(Name = "Number of Units")]
    public int? NumberOfUnits { get; set; }

    [Display(Name = "Number of Floors")]
    public int? NumberOfFloors { get; set; }
}
