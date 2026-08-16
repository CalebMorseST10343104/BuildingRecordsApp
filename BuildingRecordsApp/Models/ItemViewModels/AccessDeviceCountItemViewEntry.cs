using System;
using System.ComponentModel.DataAnnotations;
using BuildingRecordsApp.Attributes;

namespace BuildingRecordsApp.Models.ItemViewModels;

public class AccessDeviceCountItemViewEntry : ItemViewEntry
{
    [DisplayMode("Full")]
    public int? AccessDeviceCountId { get; set; }

    public int? UnitId { get; set; }

    [Display(Name = "Building Name")]
    [DisplayMode("Extended")]
    public string? BuildingName { get; set; }

    [Display(Name = "Unit Number")]
    [DisplayMode("Extended")]
    public string? UnitNumber { get; set; }

    [Display(Name = "Tags OwnershipContact")]
    [DisplayMode("Detailed")]
    public int? OwnershipContactTagCount { get; set; }

    [Display(Name = "Remotes OwnershipContact")]
    [DisplayMode("Detailed")]
    public int? OwnershipContactRemoteCount { get; set; }

    [Display(Name = "Tags Occupant")]
    [DisplayMode("Detailed")]
    public int? OccupantTagCount { get; set; }

    [Display(Name = "Remotes Occupant")]
    [DisplayMode("Detailed")]
    public int? OccupantRemoteCount { get; set; }

    [Display(Name = "Tags Agent")]
    [DisplayMode("Detailed")]
    public int? AgentTagCount { get; set; }

    [Display(Name = "Remotes Agent")]
    [DisplayMode("Detailed")]
    public int? AgentRemoteCount { get; set; }

    public override int GetId()
    {
        return AccessDeviceCountId ?? 0;
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
