using System;
using System.ComponentModel.DataAnnotations;
using BuildingRecordsApp.Attributes;
using BuildingRecordsApp.Enums;

namespace BuildingRecordsApp.Models.ItemViewModels;

public class LeaseItemViewEntry : ItemViewEntry
{
    [DisplayMode("Full")]
    public int? LeaseId { get; set; }

    [Display(Name = "Building Name")]
    [DisplayMode("Extended")]
    public string? BuildingName { get; set; }

    [Display(Name = "Unit Number")]
    [DisplayMode("Extended")]
    public string? UnitNumber { get; set; }

    [Display(Name = "Lease Holder Name")]
    [DisplayMode("Basic")]
    public string? LeaseHolderName { get; set; }

    [Display(Name = "Start Date")]
    [DisplayMode("Full")]
    public DateTime? StartDate { get; set; }

    [Display(Name = "End Date")]
    [DisplayMode("Detailed")]
    public DateTime? EndDate { get; set; }

    [Display(Name = "Number of Occupants")]
    [DisplayMode("Detailed")]
    public int? DeclaredOccupantCount { get; set; }

    [Display(Name = "Signed Conduct Rules?")]
    [DisplayMode("Full")]
    public bool? SignedRules { get; set; }

    [Display(Name = "Allowed Pets?")]
    [DisplayMode("Full")]
    public bool? PetsPresent { get; set; }

    [Display(Name = "Emergency Contact Number")]
    [DisplayMode("Full")]
    public string? EmergencyContactNumber { get; set; }

    public override int GetId()
    {
        return LeaseId ?? 0;
    }

    public override string GetTitleHeader()
    {
        return "Lease Details";
    }

    public override bool IsTitleHeaderFieldName(object item)
    {
        return false; // No specific field is used as a title header for LeaseItemViewModel
    }
}
