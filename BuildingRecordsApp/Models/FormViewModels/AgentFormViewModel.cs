using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using BuildingRecordsApp.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace BuildingRecordsApp.Models.FormViewModels;

public class AgentFormViewModel
{
    public int? AgentId { get; set; }

    [Required, Display(Name = "Person")]
    public int? PersonId { get; set; }
    public SelectList PersonSelectList { get; set; } = new SelectList(Enumerable.Empty<SelectListItem>());
    [Required, Display(Name = "Agent company")]
    public int? AgentCompanyId { get; set; }
    public SelectList AgentCompanySelectList { get; set; } = new SelectList(Enumerable.Empty<SelectListItem>());
}
