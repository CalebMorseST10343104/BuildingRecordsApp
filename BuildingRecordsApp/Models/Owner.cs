namespace BuildingRecordsApp.Models
{
    public class Owner 
    {
        public int OwnerId { get; set; }

        public Person? Person { get; set; } // Navigation property for the person
        public Ownership? Ownership { get; set; } // Navigation property for ownership
        public int? PersonId { get; set; } // Foreign key for Person
        public int? OwnershipId { get; set; } // Foreign key for Ownership
    }
}