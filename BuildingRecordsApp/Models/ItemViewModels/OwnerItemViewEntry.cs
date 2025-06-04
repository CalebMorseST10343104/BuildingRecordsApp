using System;
using System.ComponentModel.DataAnnotations;
using BuildingRecordsApp.Attributes;

namespace BuildingRecordsApp.Models.ItemViewModels;

public class OwnerItemViewEntry : ItemViewEntry
{
    [DisplayMode("Full")]
    public int? OwnerId { get; set; }

    [Display(Name = "Owner First Name")]
    [DisplayMode("Extended")]
    public string? FirstName { get; set; }

    [Display(Name = "Owner Last Name")]
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
        return OwnerId ?? 0;
    }

    public override string GetTitleHeader()
    {
        return "Owner Details";
    }

    public override bool IsTitleHeaderFieldName(object item)
    {
        return false;
    }
}
