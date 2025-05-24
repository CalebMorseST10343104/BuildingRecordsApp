using System.ComponentModel.DataAnnotations;

namespace BuildingRecordsApp.Models
{
    public class Vehicle
    {
        public int VehicleId { get; set; }

        [Display(Name = "Vehicle Registration")]
        public string VehicleRegistration { get; set; } = string.Empty;

        [Display(Name = "Vehicle Model")]
        public string VehicleModel { get; set; } = string.Empty;

        [Display(Name = "Vehicle Make")]
        public string VehicleMake { get; set; } = string.Empty;

        [Display(Name = "Vehicle Colour")]
        public string VehicleColor { get; set; } = string.Empty;

        public Unit? Unit { get; set; } // Navigation property to Unit
        public int? UnitId { get; set; } // Foreign key to Unit
    }
}