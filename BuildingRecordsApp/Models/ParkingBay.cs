namespace BuildingRecordsApp.Models
{
    public class ParkingBay
    {
        internal object Building;

        public int ParkingBayID { get; set; }
        public int ParkingBayNumber { get; set; }
        
        
        public int UnitID { get; set; } // Foreign key
        public Unit Unit { get; set; } = new(); // Navigation property

    }
}