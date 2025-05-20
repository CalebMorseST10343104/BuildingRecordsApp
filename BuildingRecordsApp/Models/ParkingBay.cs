namespace BuildingRecordsApp.Models
{
    public class ParkingBay
    {
        public int ParkingBayID { get; set; }
        public int ParkingBayNumber { get; set; }
        
        
        public int? UnitID { get; set; } // Foreign key
        public Unit? Unit { get; set; } // Navigation property

    }
}