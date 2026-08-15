namespace BuildingRecordsApp.Models.Entities;

public class OwnershipContact
{
    public int OwnershipContactId { get; set; }
    public int PersonId { get; set; }
    public int OwnershipId { get; set; }
    public Person Person { get; set; } = null!;
    public Ownership Ownership { get; set; } = null!;
}
