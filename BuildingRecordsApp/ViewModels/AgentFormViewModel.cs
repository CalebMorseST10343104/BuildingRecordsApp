using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using BuildingRecordsApp.Models;

namespace BuildingRecordsApp.ViewModels;

public class AgentFormViewModel
{
    public Agent? Agent { get; set; }
    public SelectList AgentCompanySelectList { get; set; } = new SelectList(Enumerable.Empty<SelectListItem>());
}

