using System;
using System.ComponentModel.DataAnnotations;
using BuildingRecordsApp.Attributes;
using BuildingRecordsApp.Enums;

namespace BuildingRecordsApp.Models.ItemViewModels;

public class AgentItemViewModel : ItemViewModel
{
    [DisplayMode("Full")]
    public int? AgentId { get; set; }

    [Display(Name = "First Name")]
    [DisplayMode("Basic")]
    public string? FirstName { get; set; }

    [Display(Name = "Last Name")]
    [DisplayMode("Basic")]
    public string? LastName { get; set; }

    [Display(Name = "Phone Number")]
    [DisplayMode("Detailed")]
    public string? PhoneNumber { get; set; }

    [Display(Name = "Email Address")]
    [DisplayMode("Detailed")]
    public string? Email { get; set; }

    [Display(Name = "Company Name")]
    [DisplayMode("Extended")]
    public string? CompanyName { get; set; }
}
