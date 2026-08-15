using System;
using BuildingRecordsApp.Models.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BuildingRecordsApp.Models.FormViewModels;

public class UnitFormViewModel
{
    public int? UnitId { get; set; }

    [Display(Name = "Unit Number")]
    public string? UnitNumber { get; set; }

    [Display(Name = "Bedroom Count")]
    public int? Bedrooms { get; set; }

    [Display(Name = "Has DB Inverter?")]
    public bool DbInverter { get; set; }

    [Display(Name = "Has Housekeeping?")]
    public bool Housekeeping { get; set; }

    [Display(Name = "Is Pet Friendly?")]
    public bool PetFriendly { get; set; }

    [Display(Name = "Allows Subletting?")]
    public bool SublettingAllowed { get; set; }

    [Display(Name = "AC Unit Count")]
    public int? AirconditioningUnits { get; set; }

    //Foreign keys
    public int? BuildingId { get; set; }
    public int? PrimaryContactPersonId { get; set; }
    public int? OwnershipId { get; set; }
    public int? AgentId { get; set; }
    public int? LeaseId { get; set; }
    public int? TagRemoteRecordId { get; set; }

    public SelectList BuildingSelectList { get; set; } = new SelectList(Enumerable.Empty<SelectListItem>());
}
