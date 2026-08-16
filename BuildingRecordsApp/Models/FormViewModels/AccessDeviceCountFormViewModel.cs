using System;
using BuildingRecordsApp.Models.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BuildingRecordsApp.Models.FormViewModels;

public class AccessDeviceCountFormViewModel
{
    public int? AccessDeviceCountId { get; set; }

    [Range(0, int.MaxValue), Display(Name = "Ownership contact tags")]
    public int? OwnershipContactTagCount { get; set; }

    [Range(0, int.MaxValue), Display(Name = "Ownership contact remotes")]
    public int? OwnershipContactRemoteCount { get; set; }

    [Range(0, int.MaxValue), Display(Name = "Occupant tags")]
    public int? OccupantTagCount { get; set; }

    [Range(0, int.MaxValue), Display(Name = "Occupant remotes")]
    public int? OccupantRemoteCount { get; set; }

    [Range(0, int.MaxValue), Display(Name = "Agent tags")]
    public int? AgentTagCount { get; set; }

    [Range(0, int.MaxValue), Display(Name = "Agent remotes")]
    public int? AgentRemoteCount { get; set; }

    [Required, Display(Name = "Unit")]
    public int? UnitId { get; set; } // Foreign key

    public SelectList UnitSelectList { get; set; } = new SelectList(Enumerable.Empty<SelectListItem>());
}
