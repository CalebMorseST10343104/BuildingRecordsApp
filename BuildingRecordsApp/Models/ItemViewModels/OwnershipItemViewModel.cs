using System;
using System.ComponentModel.DataAnnotations;
using BuildingRecordsApp.Attributes;

namespace BuildingRecordsApp.Models.ItemViewModels;

public class OwnershipItemViewModel : ItemViewModel
{
    public int? OwnershipId { get; set; }

    [Display(Name = "Ownership Type")]
    [DisplayMode("Basic")]
    public string? OwnershipType { get; set; }
}
