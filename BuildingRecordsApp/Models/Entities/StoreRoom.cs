using System.ComponentModel.DataAnnotations;

namespace BuildingRecordsApp.Models.Entities
{
    public class StoreRoom
    {
        public int StoreRoomId { get; set; }

        [Display(Name = "Store Room Number")]
        public string StoreRoomNumber { get; set; } = string.Empty;

        // Navigation properties
        public int PropertyId { get; set; }
        public Property Property { get; set; } = null!;
        public Unit? Unit { get; set; } // Navigation property
        public int? UnitId { get; set; } // Foreign key property
    }
}
