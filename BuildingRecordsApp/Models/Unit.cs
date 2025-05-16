namespace BuildingRecordsApp.Models
{
    public class Unit {
        public int UnitId { get; set; }
        public int UnitNumber { get; set; }
        public int Bedrooms { get; set; }
        public bool DbInverter { get; set; }
        public bool Housekeeping { get; set; }
        public bool PetFriendly { get; set; }
        public bool SublettingAllowed { get; set; }
        public int AirconditioningUnits { get; set; }

        //Navigation properties
        public Building Building { get; set; } = new();
        public Person PrimaryContactPerson { get; set; } = new();
        public Ownership Ownership { get; set; } = new();
        public Agent? Agent { get; set; }
        public Lease? Lease { get; set; }
        public TagRemoteRecord TagRemoteRecord { get; set; } = new();


        //Foreign keys
        public int BuildingId { get; set; }
        public int PrimaryContactPersonId { get; set; }
        public int OwnershipId { get; set; }
        public int? AgentId { get; set; }
        public int? LeaseId { get; set; }
        public int TagRemoteRecordId { get; set; }
        
    }
}