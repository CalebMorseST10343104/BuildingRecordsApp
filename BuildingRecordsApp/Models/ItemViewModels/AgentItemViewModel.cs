using System;
using System.ComponentModel.DataAnnotations;

namespace BuildingRecordsApp.Models.ItemViewModels;

public class AgentItemViewModel
{
    public int? AgentId { get; set; }

    [Display(Name = "First Name")]
    public string? FirstName { get; set; }

    [Display(Name = "Last Name")]
    public string? LastName { get; set; }

    [Display(Name = "Phone Number")]
    public string? PhoneNumber { get; set; }

    [Display(Name = "Email Address")]
    public string? Email { get; set; }
    
    //Related model display fields
    [Display(Name = "Company Name")]
    public string? CompanyName { get; set; }
}
