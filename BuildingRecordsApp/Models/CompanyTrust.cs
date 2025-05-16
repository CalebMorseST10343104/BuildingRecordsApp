namespace BuildingRecordsApp.Models
{
    public class CompanyTrust
    {
        public int CompanyTrustId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string RegistrationNumber { get; set; } = string.Empty;

        public ICollection<Ownership> Ownerships { get; set; } = [];
        
    }
}