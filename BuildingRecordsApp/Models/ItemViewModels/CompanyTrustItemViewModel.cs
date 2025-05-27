using System;
using System.ComponentModel.DataAnnotations;

namespace BuildingRecordsApp.Models.ItemViewModels;

public class CompanyTrustItemViewModel
{
    public int? CompanyTrustId { get; set; }
    
    [Display(Name = "Company/Trust Name")]
    public string? Name { get; set; }

    [Display(Name = "Address")]
    public string? Address { get; set; }

    [Display(Name = "Registration Number")]
    public string? RegistrationNumber { get; set; }
}
