using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BuildingRecordsApp.Models
{
    public class Agent
    {
        public int AgentId { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;

        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty;

        public AgentCompany? AgentCompany { get; set; } // Navigation property
        public ICollection<Unit> Units { get; set; } = []; // Navigation property

        public int? AgentCompanyId { get; set; } // Foreign key to AgentCompany
    }
}