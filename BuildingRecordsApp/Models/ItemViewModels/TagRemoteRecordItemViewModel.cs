using System;
using System.ComponentModel.DataAnnotations;

namespace BuildingRecordsApp.Models.ItemViewModels;

public class TagRemoteRecordItemViewModel
{
    public int? TagRemoteRecordId { get; set; }

    [Display(Name = "Tags Owner")]
    public int? TagsOwner { get; set; }

    [Display(Name = "Remotes Owner")]
    public int? RemotesOwner { get; set; }

    [Display(Name = "Tags Occupant")]
    public int? TagsOccupant { get; set; }

    [Display(Name = "Remotes Occupant")]
    public int? RemotesOccupant { get; set; }

    [Display(Name = "Tags Agent")]
    public int? TagsAgent { get; set; }

    [Display(Name = "Remotes Agent")]
    public int? RemotesAgent { get; set; }

    public Dictionary<string, object?> ToDictionary()
    {
        return new Dictionary<string, object?>
        {
            { nameof(TagRemoteRecordId), TagRemoteRecordId },
            { nameof(TagsOwner), TagsOwner },
            { nameof(RemotesOwner), RemotesOwner },
            { nameof(TagsOccupant), TagsOccupant },
            { nameof(RemotesOccupant), RemotesOccupant },
            { nameof(TagsAgent), TagsAgent },
            { nameof(RemotesAgent), RemotesAgent }
        };
    }
}
