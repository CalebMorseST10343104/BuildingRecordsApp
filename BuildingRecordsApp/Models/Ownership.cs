using System.ComponentModel.DataAnnotations;

namespace BuildingRecordsApp.Models
{
    public class Ownership
    {
        public int OwnershipId { get; set; }

        [Display(Name = "Ownership Type")]
        public string OwnershipType { get; set; } = string.Empty; // e.g., "Natural", "Juristic"

        public int? UnitId { get; set; } // Foreign key
        public int? CompanyTrustId { get; set; } // Foreign key for CompanyTrust

        public Unit? Unit { get; set; } // Navigation property
        public CompanyTrust? CompanyTrust { get; set; } // Navigation property for CompanyTrust
        public ICollection<Owner> Owners { get; set; } = []; // Navigation property for owners
    }
}