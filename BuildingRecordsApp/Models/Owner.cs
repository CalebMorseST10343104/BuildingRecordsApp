namespace BuildingRecordsApp.Models
{
    public class Owner 
    {
        public int OwnerId { get; set; }

        public Person Person { get; set; } = new(); // Navigation property for the person
        public int PersonId { get; set; } // Foreign key for Person
        public Ownership Ownership { get; set; } = new(); // Navigation property for ownership
        public int OwnershipId { get; set; } // Foreign key for Ownership
    }
}