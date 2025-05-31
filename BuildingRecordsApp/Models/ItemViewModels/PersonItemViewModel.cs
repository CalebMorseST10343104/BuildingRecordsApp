using System;
using System.ComponentModel.DataAnnotations;
using BuildingRecordsApp.Attributes;

namespace BuildingRecordsApp.Models.ItemViewModels;

public class PersonItemViewModel : ItemViewModel
{
    [DisplayMode("Full")]
    public int? PersonId { get; set; }

    [Display(Name = "First Name")]
    [DisplayMode("Basic")]
    public string? FirstName { get; set; }

    [Display(Name = "Last Name")]
    [DisplayMode("Basic")]
    public string? LastName { get; set; }

    [Display(Name = "Email Address")]
    [DisplayMode("Detailed")]
    public string? Email { get; set; }

    [Display(Name = "Postal Address")]
    [DisplayMode("Detailed")]
    public string? PostalAddress { get; set; }

    [Display(Name = "ID Number")]
    [DisplayMode("Full")]
    public string? IdNumber { get; set; }

    [Display(Name = "Phone Number")]
    [DisplayMode("Full")]
    public string? PhoneNumber { get; set; }
}
