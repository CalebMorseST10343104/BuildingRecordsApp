using System.ComponentModel.DataAnnotations;

namespace BuildingRecordsApp.Models
{
    public class Person
    {
        public int PersonId { get; set; }

        [Display(Name = "First Name")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Last Name")]
        public string Surname { get; set; } = string.Empty;

        [Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Postal Address")]
        public string PostalAddress { get; set; } = string.Empty;

        [Display(Name = "ID Number")]
        public string IdNumber { get; set; } = string.Empty;

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; } = string.Empty;

        // Navigation properties
        public ICollection<Occupancy> Occupancies { get; set; } = [];
        public ICollection<Owner> Owners { get; set; } = [];
        public ICollection<Unit> PrimaryContactUnits { get; set; } = [];
    }
}