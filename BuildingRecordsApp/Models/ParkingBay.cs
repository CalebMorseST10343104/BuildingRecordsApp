using System.ComponentModel.DataAnnotations;

namespace BuildingRecordsApp.Models
{
    public class ParkingBay
    {
        public int ParkingBayID { get; set; }

        [Display(Name = "Parking Bay Number")]
        public string ParkingBayNumber { get; set; } = string.Empty;


        public int? UnitID { get; set; } // Foreign key
        public Unit? Unit { get; set; } // Navigation property

    }
}