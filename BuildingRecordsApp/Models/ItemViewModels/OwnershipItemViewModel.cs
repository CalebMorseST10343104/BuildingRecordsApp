using System;
using System.ComponentModel.DataAnnotations;

namespace BuildingRecordsApp.Models.ItemViewModels;

public class OwnershipItemViewModel
{
    public int? OwnershipId { get; set; }

    [Display(Name = "Ownership Type")]
    public string? OwnershipType { get; set; } 
}
