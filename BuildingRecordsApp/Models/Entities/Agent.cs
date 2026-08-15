namespace BuildingRecordsApp.Models.Entities
{
    public class Agent
    {
        public int AgentId { get; set; }

        public int PersonId { get; set; }
        public int AgentCompanyId { get; set; }
        public Person Person { get; set; } = null!;
        public AgentCompany AgentCompany { get; set; } = null!;
        public ICollection<Unit> Units { get; set; } = []; // Navigation property
    }
}
