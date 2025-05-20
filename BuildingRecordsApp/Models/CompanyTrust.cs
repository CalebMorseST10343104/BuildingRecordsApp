using System.ComponentModel.DataAnnotations;

namespace BuildingRecordsApp.Models
{
    public class CompanyTrust
    {
        public int CompanyTrustId { get; set; }

        [Display(Name = "Company/Trust Name")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Address")]
        public string Address { get; set; } = string.Empty;

        [Display(Name = "Registration Number")]
        public string RegistrationNumber { get; set; } = string.Empty;

        public ICollection<Ownership> Ownerships { get; set; } = [];

    }
}