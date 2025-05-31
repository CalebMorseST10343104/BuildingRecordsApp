using System;
using System.ComponentModel.DataAnnotations;
using BuildingRecordsApp.Attributes;

namespace BuildingRecordsApp.Models.ItemViewModels;

public class TagRemoteRecordItemViewModel : ItemViewModel, IItemViewModel
{
    [DisplayMode("Full")]
    public int? TagRemoteRecordId { get; set; }

    [Display(Name = "Building Name")]
    [DisplayMode("Extended")]
    public string? BuildingName { get; set; }

    [Display(Name = "Unit Number")]
    [DisplayMode("Extended")]
    public int? UnitNumber { get; set; }

    [Display(Name = "Tags Owner")]
    [DisplayMode("Detailed")]
    public int? TagsOwner { get; set; }

    [Display(Name = "Remotes Owner")]
    [DisplayMode("Detailed")]
    public int? RemotesOwner { get; set; }

    [Display(Name = "Tags Occupant")]
    [DisplayMode("Detailed")]
    public int? TagsOccupant { get; set; }

    [Display(Name = "Remotes Occupant")]
    [DisplayMode("Detailed")]
    public int? RemotesOccupant { get; set; }

    [Display(Name = "Tags Agent")]
    [DisplayMode("Detailed")]
    public int? TagsAgent { get; set; }

    [Display(Name = "Remotes Agent")]
    [DisplayMode("Detailed")]
    public int? RemotesAgent { get; set; }

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
