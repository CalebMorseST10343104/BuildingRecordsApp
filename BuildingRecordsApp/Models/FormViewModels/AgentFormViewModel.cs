using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using BuildingRecordsApp.Models.Entities;

namespace BuildingRecordsApp.Models.FormViewModels;

public class AgentFormViewModel
{
    public Agent? Agent { get; set; }
    public SelectList AgentCompanySelectList { get; set; } = new SelectList(Enumerable.Empty<SelectListItem>());
}

