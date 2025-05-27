using System;
using System.ComponentModel.DataAnnotations;

namespace BuildingRecordsApp.Models.ItemViewModels;

public class PersonItemViewModel
{
    public int? PersonId { get; set; }

    [Display(Name = "First Name")]
    public string? Name { get; set; }

    [Display(Name = "Last Name")]
    public string? Surname { get; set; }

    [Display(Name = "Email Address")]
    public string? Email { get; set; }

    [Display(Name = "Postal Address")]
    public string? PostalAddress { get; set; }

    [Display(Name = "ID Number")]
    public string? IdNumber { get; set; }

    [Display(Name = "Phone Number")]
    public string? PhoneNumber { get; set; }
}
