using System;
using BuildingRecordsApp.Models.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BuildingRecordsApp.Models.FormViewModels;

public class TagRemoteRecordFormViewModel
{
    public int? TagRemoteRecordId { get; set; }

    [Display(Name = "Tags OwnershipContact")]
    public int? TagsOwner { get; set; }

    [Display(Name = "Remotes OwnershipContact")]
    public int? RemotesOwner { get; set; }

    [Display(Name = "Tags Occupant")]
    public int? TagsOccupant { get; set; }

    [Display(Name = "Remotes Occupant")]
    public int? RemotesOccupant { get; set; }

    [Display(Name = "Tags Agent")]
    public int? TagsAgent { get; set; }

    [Display(Name = "Remotes Agent")]
    public int? RemotesAgent { get; set; }

    public int? UnitId { get; set; } // Foreign key

    public SelectList UnitSelectList { get; set; } = new SelectList(Enumerable.Empty<SelectListItem>());
}
