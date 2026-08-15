using System;
using BuildingRecordsApp.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace BuildingRecordsApp.Models.FormViewModels;

public class CompanyTrustFormViewModel
{
    public int? OrganizationId { get; set; }

    [Display(Name = "Company/Trust Name")]
    public string? Name { get; set; }

    [Display(Name = "Address")]
    public string? Address { get; set; }

    [Display(Name = "Registration Number")]
    public string? RegistrationNumber { get; set; }
}
