using System;
using System.ComponentModel.DataAnnotations;

namespace BuildingRecordsApp.Models.ItemViewModels;

public class PersonItemViewModel
{
    public int? PersonId { get; set; }

    [Display(Name = "First Name")]
    public string? FirstName { get; set; }

    [Display(Name = "Last Name")]
    public string? LastName { get; set; }

    [Display(Name = "Email Address")]
    public string? Email { get; set; }

    [Display(Name = "Postal Address")]
    public string? PostalAddress { get; set; }

    [Display(Name = "ID Number")]
    public string? IdNumber { get; set; }

    [Display(Name = "Phone Number")]
    public string? PhoneNumber { get; set; }

    public Dictionary<string, object?> ToDictionary()
    {
        return new Dictionary<string, object?>
        {
            { nameof(PersonId), PersonId },
            { nameof(FirstName), FirstName },
            { nameof(LastName), LastName },
            { nameof(Email), Email },
            { nameof(PostalAddress), PostalAddress },
            { nameof(IdNumber), IdNumber },
            { nameof(PhoneNumber), PhoneNumber }
        };
    }
}
