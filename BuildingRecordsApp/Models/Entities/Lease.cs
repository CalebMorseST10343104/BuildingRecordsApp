using System.ComponentModel.DataAnnotations;

namespace BuildingRecordsApp.Models.Entities
{
    public class Lease
    {
        public int LeaseId { get; set; }

        [Display(Name = "Lease Holder Name")]
        public string LeaseHolderName { get; set; } = string.Empty;

        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Declared Occupant Count")]
        public int DeclaredOccupantCount { get; set; }

        [Display(Name = "Signed Conduct Rules?")]
        public bool SignedRules { get; set; }

        [Display(Name = "Pets Present?")]
        public bool PetsPresent { get; set; }

        [Display(Name = "Emergency Contact Number")]
        public string EmergencyContactNumber { get; set; } = string.Empty;

        public Unit? Unit { get; set; } // Navigation property

        public int? UnitId { get; set; } // Foreign key to Unit
    }
}
