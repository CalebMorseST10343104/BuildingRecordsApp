using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using BuildingRecordsApp.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace BuildingRecordsApp.Models.FormViewModels;

public class AgentFormViewModel
{
    public int? AgentId { get; set; }

    [Display(Name = "First Name")]
    public string? FirstName { get; set; }

    [Display(Name = "Last Name")]
    public string? LastName { get; set; }

    [Display(Name = "Phone Number")]
    public string? PhoneNumber { get; set; }

    [Display(Name = "Email Address")]
    public string? Email { get; set; }
    public int? AgentCompanyId { get; set; }
    public SelectList AgentCompanySelectList { get; set; } = new SelectList(Enumerable.Empty<SelectListItem>());
}

