namespace BuildingRecordsApp.Models
{
    public class Lease
    {
        public int LeaseId { get; set; }
        public string LeaseHolderName { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int PersonsOccupying { get; set; }
        public bool SignedRules { get; set; }
        public bool AllowedPets { get; set; }
        public string EmergencyContactNumber { get; set; } = string.Empty;

        public Unit? Unit { get; set; } // Navigation property

        public int? UnitId { get; set; } // Foreign key to Unit
    }
}