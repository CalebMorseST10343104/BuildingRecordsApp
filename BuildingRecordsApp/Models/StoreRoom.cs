public class StoreRoom
{
    public int StoreRoomId { get; set; }
    public string StoreRoomNumber { get; set; } = string.Empty;

    // Navigation properties
    public Unit Unit { get; set; } = new(); // Navigation property
    public int UnitId { get; set; } // Foreign key property
}