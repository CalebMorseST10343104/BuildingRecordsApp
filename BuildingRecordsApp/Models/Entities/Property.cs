using System.ComponentModel.DataAnnotations;

namespace BuildingRecordsApp.Models.Entities;

public class Property
{
    public int PropertyId { get; set; }
    [Display(Name = "Property Name")]
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public ICollection<Building> Buildings { get; set; } = [];
    public ICollection<ParkingBay> ParkingBays { get; set; } = [];
    public ICollection<StoreRoom> StoreRooms { get; set; } = [];
}
