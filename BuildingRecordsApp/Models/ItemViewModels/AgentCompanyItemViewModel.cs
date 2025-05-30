using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BuildingRecordsApp.Models.ItemViewModels;

public class AgentCompanyItemViewModel
{
    public int? AgentCompanyId { get; set; }

    [Display(Name = "Company Name")]
    public string? CompanyName { get; set; }

    [Display(Name = "Address")]
    public string? Address { get; set; }

    [Display(Name = "Registration Number")]
    public string? RegistrationNumber { get; set; }

    public Dictionary<string, object?> ToDictionary()
    {
        return new Dictionary<string, object?>
        {
            { nameof(AgentCompanyId), AgentCompanyId },
            { nameof(CompanyName), CompanyName },
            { nameof(Address), Address },
            { nameof(RegistrationNumber), RegistrationNumber }
        };
    }
}
