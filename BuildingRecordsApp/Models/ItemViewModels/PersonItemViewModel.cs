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
    [DisplayMode("Full")]
    public string? PostalAddress { get; set; }

    [Display(Name = "ID Number")]
    [DisplayMode("Full")]
    public string? IdNumber { get; set; }

    [Display(Name = "Phone Number")]
    [DisplayMode("Detailed")]
    public string? PhoneNumber { get; set; }

    public override string GetTitleHeader()
    {
        if (string.IsNullOrEmpty(FirstName) && string.IsNullOrEmpty(LastName))
        {
            return "Person";
        }
        else
        {
            return $"{FirstName} {LastName}".Trim();
        }
    }

    public override bool IsTitleHeaderFieldName(object item)
    {
        return item is nameof(FirstName) || item is nameof(LastName);
    }
}
