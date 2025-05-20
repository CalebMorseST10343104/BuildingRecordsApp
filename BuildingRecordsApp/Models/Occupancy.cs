namespace BuildingRecordsApp.Models
{
    public class Occupancy
    {
        public int OccupancyId { get; set; }
        public string OccupationType { get; set; } = string.Empty; // e.g., "Owner", "Short-Term Rental", "Long-Term Rental"

        public Unit? Unit { get; set; } // Navigation property for the unit
        public Person? Occupant { get; set; } // Navigation property for occupants
        
        public int? UnitId { get; set; } // Foreign key for Unit
        public int? OccupantId { get; set; } // Foreign key for Person
    }
}