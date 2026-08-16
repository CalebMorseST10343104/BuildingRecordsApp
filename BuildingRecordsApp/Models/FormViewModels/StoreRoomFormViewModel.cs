using System;
using BuildingRecordsApp.Models.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BuildingRecordsApp.Models.FormViewModels;

public class StoreRoomFormViewModel
{
    public int? StoreRoomId { get; set; }
    public int PropertyId { get; set; }
    public SelectList PropertySelectList { get; set; } = new SelectList(Enumerable.Empty<SelectListItem>());

    [Required, Display(Name = "Store Room Number")]
    public string? StoreRoomNumber { get; set; }

    public int? UnitId { get; set; } // Foreign key property

    public SelectList UnitSelectList { get; set; } = new SelectList(Enumerable.Empty<SelectListItem>());
}
