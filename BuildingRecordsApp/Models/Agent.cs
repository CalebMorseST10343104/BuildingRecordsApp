namespace BuildingRecordsApp.Models
{
    public class Agent 
    {
        public int AgentId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;


        public AgentCompany? AgentCompany { get; set; } // Navigation property
        public ICollection<Unit> Units { get; set; } = []; // Navigation property

        public int? AgentCompanyId { get; set; } // Foreign key to AgentCompany
    }
}