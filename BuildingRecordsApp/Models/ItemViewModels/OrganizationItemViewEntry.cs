using System;
using System.ComponentModel.DataAnnotations;
using BuildingRecordsApp.Attributes;
using BuildingRecordsApp.Enums;

namespace BuildingRecordsApp.Models.ItemViewModels;

public class OrganizationItemViewEntry : ItemViewEntry
{
    [DisplayMode("Full")]
    public int? OrganizationId { get; set; }

    [Display(Name = "Company/Trust Name")]
    [DisplayMode("Basic")]
    public string? Name { get; set; }

    [Display(Name = "Address")]
    [DisplayMode("Detailed")]
    public string? Address { get; set; }

    [Display(Name = "Registration Number")]
    [DisplayMode("Detailed")]
    public string? RegistrationNumber { get; set; }

    public override int GetId()
    {
        return OrganizationId ?? 0;
    }

    public override string GetTitleHeader()
    {
        if (string.IsNullOrEmpty(Name))
        {
            return "Company/Trust";
        }
        else
        {
            return Name;
        }
    }

    public override bool IsTitleHeaderFieldName(object item)
    {
        return item is nameof(Name);
    }
}
