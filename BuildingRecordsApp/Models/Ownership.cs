using System.ComponentModel.DataAnnotations;

namespace BuildingRecordsApp.Models
{
    public class Ownership
    {
        public int OwnershipId { get; set; }

        [Display(Name = "Ownership Type")]
        public string OwnershipType { get; set; } = string.Empty; // e.g., "Natural", "Juristic"

        public int? UnitId { get; set; } // Foreign key

        public Unit? Unit { get; set; } // Navigation property
        public ICollection<Person>? Owners { get; set; } // Navigation property for owners
    }
}