using System;
using System.ComponentModel.DataAnnotations;
using BuildingRecordsApp.Attributes;
using BuildingRecordsApp.Enums;

namespace BuildingRecordsApp.Models.ItemViewModels;

public class BuildingItemViewModel : ItemViewModel
{
    [DisplayMode("Full")]
    public int? BuildingId { get; set; }

    [Display(Name = "Building Name")]
    [DisplayMode("Basic")]
    public string? Name { get; set; }

    [Display(Name = "Address")]
    [DisplayMode("Detailed")]
    public string? Address { get; set; }

    [Display(Name = "Number of Units")]
    [DisplayMode("Detailed")]
    public int? NumberOfUnits { get; set; }

    [Display(Name = "Number of Floors")]
    [DisplayMode("Detailed")]
    public int? NumberOfFloors { get; set; }

    public override int GetId()
    {
        return BuildingId ?? 0;
    }

    public override string GetTitleHeader()
    {
        if (string.IsNullOrEmpty(Name))
        {
            return "Building";
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
