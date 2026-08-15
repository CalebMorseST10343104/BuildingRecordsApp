using System.ComponentModel.DataAnnotations;

namespace BuildingRecordsApp.Models.Entities
{
    public class Ownership
    {
        public int OwnershipId { get; set; }

        [Display(Name = "Ownership Type")]
        public string OwnershipType { get; set; } = string.Empty; // e.g., "Natural", "Juristic"

        public int? UnitId { get; set; } // Foreign key
        public int? OrganizationId { get; set; }

        public Unit? Unit { get; set; } // Navigation property
        public Organization? Organization { get; set; }
        public ICollection<OwnershipContact> OwnershipContacts { get; set; } = [];
    }
}
