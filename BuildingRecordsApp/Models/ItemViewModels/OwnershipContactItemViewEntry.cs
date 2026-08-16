using System;
using System.ComponentModel.DataAnnotations;
using BuildingRecordsApp.Attributes;

namespace BuildingRecordsApp.Models.ItemViewModels;

public class OwnershipContactItemViewEntry : ItemViewEntry
{
    [DisplayMode("Full")]
    public int? OwnershipContactId { get; set; }

    [Display(Name = "Property")]
    [DisplayMode("Extended")]
    public string? PropertyName { get; set; }

    [Display(Name = "First Name")]
    [DisplayMode("Extended")]
    public string? FirstName { get; set; }

    [Display(Name = "Last Name")]
    [DisplayMode("Extended")]
    public string? LastName { get; set; }

    [Display(Name = "Building Name")]
    [DisplayMode("Extended")]
    public string? BuildingName { get; set; }

    [Display(Name = "Unit Number")]
    [DisplayMode("Extended")]
    public string? UnitNumber { get; set; }

    public override int GetId()
    {
        return OwnershipContactId ?? 0;
    }

    public override string GetTitleHeader()
    {
        return "OwnershipContact Details";
    }

    public override bool IsTitleHeaderFieldName(object item)
    {
        return false;
    }
}
