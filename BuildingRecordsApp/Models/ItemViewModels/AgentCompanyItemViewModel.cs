using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using BuildingRecordsApp.Attributes;
using BuildingRecordsApp.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BuildingRecordsApp.Models.ItemViewModels;

public class AgentCompanyItemViewModel : ItemViewModel
{
    [DisplayMode("Full")]
    public int? AgentCompanyId { get; set; }

    [Display(Name = "Company Name")]
    [DisplayMode("Basic")]
    public string? CompanyName { get; set; }

    [Display(Name = "Address")]
    [DisplayMode("Detailed")]
    public string? Address { get; set; }

    [Display(Name = "Registration Number")]
    [DisplayMode("Detailed")]
    public string? RegistrationNumber { get; set; }
}
