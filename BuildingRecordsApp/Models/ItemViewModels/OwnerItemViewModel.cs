using System;
using System.ComponentModel.DataAnnotations;
using BuildingRecordsApp.Attributes;

namespace BuildingRecordsApp.Models.ItemViewModels;

public class OwnerItemViewModel : ItemViewModel, IItemViewModel
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

    public string GetTitleHeader(string valueIfNull)
    {
        throw new NotImplementedException();
    }

    public string GetTitleHeaderFieldName(bool formatted = false)
    {
        throw new NotImplementedException();
    }

    public bool HasTitleHeader()
    {
        throw new NotImplementedException();
    }
}
