using System.ComponentModel.DataAnnotations;

namespace BuildingRecordsApp.Models
{
    public class Building
    {
        public int BuildingId { get; set; }

        [Display(Name = "Building Name")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Address")]
        public string Address { get; set; } = string.Empty;

        [Display(Name = "Number of Units")]
        public int NumberOfUnits { get; set; }

        [Display(Name = "Number of Floors")]
        public int NumberOfFloors { get; set; }

        //Navigation properties
        public ICollection<Unit> Units { get; set; } = [];
    }
}