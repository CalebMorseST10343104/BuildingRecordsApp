namespace BuildingRecordsApp.Models
{
    public class Ownership
    {
        public int OwnershipId { get; set; }
        public string OwnershipType { get; set; } = string.Empty; // e.g., "Natural", "Juristic"

        public string UnitId { get; set; } = string.Empty; // Foreign key
        public Unit Unit { get; set; } = new(); // Navigation property
    }
}