using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BuildingRecordsApp.Models
{
    public class AgentCompany
    {
        public int AgentCompanyId { get; set; }

        [Display(Name = "Company Name")]
        public string CompanyName { get; set; } = string.Empty;

        [Display(Name = "Address")]
        public string Address { get; set; } = string.Empty;

        [Display(Name = "Registration Number")]
        public string RegistrationNumber { get; set; } = string.Empty;

        // Navigation property
        public ICollection<Person> Agents { get; set; } = [];
    }
}