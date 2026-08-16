using System;
using BuildingRecordsApp.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace BuildingRecordsApp.Models.FormViewModels;

public class AgentCompanyFormViewModel
{
    public int? AgentCompanyId { get; set; }

    [Required, Display(Name = "Company Name")]
    public string? CompanyName { get; set; }

    [Display(Name = "Address")]
    public string? Address { get; set; }

    [Display(Name = "Registration Number")]
    public string? RegistrationNumber { get; set; }
}
