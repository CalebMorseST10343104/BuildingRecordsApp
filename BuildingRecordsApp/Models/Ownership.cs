namespace BuildingRecordsApp.Models
{
    public class Ownership
    {
        public int OwnershipId { get; set; }
        public string OwnershipType { get; set; } = string.Empty; // e.g., "Natural", "Juristic"

        public int? UnitId { get; set; } // Foreign key
        public Unit? Unit { get; set; } // Navigation property
        
    }
}