using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using BuildingRecordsApp.Models.Entities;

namespace BuildingRecordsApp.Models.FormViewModels;

public class OwnerFormViewModel
{
    public Owner? Owner { get; set; }
    public SelectList OwnershipSelectList { get; set; } = new SelectList(Enumerable.Empty<SelectListItem>());
    public SelectList PersonSelectList { get; set; } = new SelectList(Enumerable.Empty<SelectListItem>());
    

}
