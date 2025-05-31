using System;
using System.ComponentModel.DataAnnotations;
using BuildingRecordsApp.Attributes;
using BuildingRecordsApp.Enums;

namespace BuildingRecordsApp.Models.ItemViewModels;

public class CompanyTrustItemViewModel : ItemViewModel
{
    [DisplayMode("Full")]
    public int? CompanyTrustId { get; set; }

    [Display(Name = "Company/Trust Name")]
    [DisplayMode("Basic")]
    public string? Name { get; set; }

    [Display(Name = "Address")]
    [DisplayMode("Detailed")]
    public string? Address { get; set; }

    [Display(Name = "Registration Number")]
    [DisplayMode("Detailed")]
    public string? RegistrationNumber { get; set; }
}
