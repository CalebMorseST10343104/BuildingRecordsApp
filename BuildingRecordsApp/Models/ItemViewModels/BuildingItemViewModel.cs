using System;
using System.ComponentModel.DataAnnotations;
using BuildingRecordsApp.Attributes;
using BuildingRecordsApp.Enums;

namespace BuildingRecordsApp.Models.ItemViewModels;

public class BuildingItemViewModel : ItemViewModel, IItemViewModel
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
