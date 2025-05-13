public class Occupancy
{
    public int OccupancyId { get; set; }
    public string OccupationType { get; set; } = string.Empty; // e.g., "Owner", "Short-Term Rental", "Long-Term Rental"
    public int UnitNumber { get; set; } // Foreign key for Unit
    public Unit Unit { get; set; } = new(); // Navigation property for the unit
    public ICollection<Person> Occupants { get; set; } = []; // Navigation property for occupants
}