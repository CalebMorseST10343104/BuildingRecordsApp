using System;
using System.ComponentModel.DataAnnotations;

namespace BuildingRecordsApp.Models.ItemViewModels;

public class LeaseItemViewModel
{
    public int? LeaseId { get; set; }

    [Display(Name = "Lease Holder Name")]
    public string? LeaseHolderName { get; set; }

    [Display(Name = "Start Date")]
    public DateTime? StartDate { get; set; }

    [Display(Name = "End Date")]
    public DateTime? EndDate { get; set; }

    [Display(Name = "Number of Occupants")]
    public int? PersonsOccupying { get; set; }

    [Display(Name = "Signed Conduct Rules?")]
    public bool? SignedRules { get; set; }

    [Display(Name = "Allowed Pets?")]
    public bool? AllowedPets { get; set; }

    [Display(Name = "Emergency Contact Number")]
    public string? EmergencyContactNumber { get; set; }

    public Dictionary<string, object?> LeaseDetails()
    {
        return new Dictionary<string, object?>
        {
            { nameof(LeaseId), LeaseId },
            { nameof(LeaseHolderName), LeaseHolderName },
            { nameof(StartDate), StartDate },
            { nameof(EndDate), EndDate },
            { nameof(PersonsOccupying), PersonsOccupying },
            { nameof(SignedRules), SignedRules },
            { nameof(AllowedPets), AllowedPets },
            { nameof(EmergencyContactNumber), EmergencyContactNumber }
        };
    }
}
