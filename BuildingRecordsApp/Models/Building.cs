namespace BuildingRecordsApp.Models
{
    public class Building
    {
        public int BuildingId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public int NumberOfUnits { get; set; }
        public int NumberOfFloors { get; set; }
        
        //Navigation properties
        public ICollection<Unit> Units { get; set; } = [];
    }
}