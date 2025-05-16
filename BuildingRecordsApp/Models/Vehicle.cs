namespace BuildingRecordsApp.Models
{
    public class Vehicle
    {
        public int VehicleId { get; set; }
        public string VehicleRegistration { get; set; } = string.Empty;
        public string VehicleModel { get; set; } = string.Empty;
        public string VehicleMake { get; set; } = string.Empty;
        public string VehicleColor { get; set; } = string.Empty;
        
        public Unit Unit { get; set; } = new(); // Navigation property to Unit
        public int UnitId { get; set; } // Foreign key to Unit
    }
}