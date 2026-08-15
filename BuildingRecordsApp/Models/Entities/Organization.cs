using System.ComponentModel.DataAnnotations;

namespace BuildingRecordsApp.Models.Entities;

public class Organization
{
    public int OrganizationId { get; set; }
    [Display(Name = "Organization Name")]
    public string Name { get; set; } = string.Empty;
    public string? OrganizationType { get; set; }
    public string? RegistrationReference { get; set; }
    public string? Country { get; set; }
    public string? Address { get; set; }
    public ICollection<Ownership> Ownerships { get; set; } = [];
}
