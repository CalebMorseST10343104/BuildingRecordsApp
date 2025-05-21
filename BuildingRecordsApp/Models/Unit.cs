using System.ComponentModel.DataAnnotations;

namespace BuildingRecordsApp.Models
{
    public class Unit
    {
        public int UnitId { get; set; }

        [Display(Name = "Unit Number")]
        public string UnitNumber { get; set; } = string.Empty;

        [Display(Name = "Bedroom Count")]
        public int Bedrooms { get; set; }

        [Display(Name = "Has DB Inverter?")]
        public bool DbInverter { get; set; }

        [Display(Name = "Has Housekeeping?")]
        public bool Housekeeping { get; set; }

        [Display(Name = "Is Pet Friendly?")]
        public bool PetFriendly { get; set; }

        [Display(Name = "Allows Subletting?")]
        public bool SublettingAllowed { get; set; }

        [Display(Name = "AC Unit Count")]
        public int AirconditioningUnits { get; set; }

        //Navigation properties
        public Building? Building { get; set; }
        public Person? PrimaryContactPerson { get; set; }
        public Ownership? Ownership { get; set; }
        public Agent? Agent { get; set; }
        public Lease? Lease { get; set; }
        public TagRemoteRecord? TagRemoteRecord { get; set; }
        public ICollection<Occupancy> Occupants { get; set; } = [];
        public ICollection<ParkingBay> ParkingBays { get; set; } = [];
        public ICollection<StoreRoom> StoreRooms { get; set; } = [];
        public ICollection<Vehicle> Vehicles { get; set; } = [];


        //Foreign keys
        public int? BuildingId { get; set; }
        public int? PrimaryContactPersonId { get; set; }
        public int? OwnershipId { get; set; }
        public int? AgentId { get; set; }
        public int? LeaseId { get; set; }
        public int? TagRemoteRecordId { get; set; }

    }
}