using System;
using System.ComponentModel.DataAnnotations;
using BuildingRecordsApp.Attributes;

namespace BuildingRecordsApp.Models.ItemViewModels;

public class TagRemoteRecordItemViewModel : ItemViewModel
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

    public override int GetId()
    {
        return TagRemoteRecordId ?? 0;
    }

    public override string GetTitleHeader()
    {
        return "Tag Remote Record Details";
    }

    public override bool IsTitleHeaderFieldName(object item)
    {
        return false;
    }
}
