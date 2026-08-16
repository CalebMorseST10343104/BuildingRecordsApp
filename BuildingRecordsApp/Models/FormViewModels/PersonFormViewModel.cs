using System;
using BuildingRecordsApp.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace BuildingRecordsApp.Models.FormViewModels;

public class PersonFormViewModel
{
    public int? PersonId { get; set; }

    [Required, Display(Name = "First Name")]
    public string? FirstName { get; set; }

    [Required, Display(Name = "Last Name")]
    public string? LastName { get; set; }

    [EmailAddress, Display(Name = "Email Address")]
    public string? Email { get; set; }

    [Display(Name = "Postal Address")]
    public string? PostalAddress { get; set; }

    [Display(Name = "ID Number")]
    public string? IdNumber { get; set; }

    [Phone, Display(Name = "Phone Number")]
    public string? PhoneNumber { get; set; }
}
