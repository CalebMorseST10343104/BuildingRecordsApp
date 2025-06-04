using System;
using System.ComponentModel.DataAnnotations;
using BuildingRecordsApp.Attributes;

namespace BuildingRecordsApp.Models.ItemViewModels;

public class OwnershipItemViewEntry : ItemViewEntry
{
    [DisplayMode("Full")]
    public int? OwnershipId { get; set; }

    [Display(Name = "Building Name")]
    [DisplayMode("Extended")]
    public string? BuildingName { get; set; }

    [Display(Name = "Unit Number")]
    [DisplayMode("Extended")]
    public int? UnitNumber { get; set; }

    [Display(Name = "Company Name")]
    [DisplayMode("Extended")]
    public string? CompanyName { get; set; }

    [Display(Name = "Ownership Type")]
    [DisplayMode("Basic")]
    public string? OwnershipType { get; set; }

    public override int GetId()
    {
        return OwnershipId ?? 0;
    }

    public override string GetTitleHeader()
    {
        if (string.IsNullOrEmpty(BuildingName) && UnitNumber == null)
        {
            return "Ownership Details";
        }
        else
        {
            return $"{BuildingName} - Unit {UnitNumber}".Trim();
        }
    }

    public override bool IsTitleHeaderFieldName(object item)
    {
        return item is nameof(BuildingName) || item is nameof(UnitNumber);
    }
}
