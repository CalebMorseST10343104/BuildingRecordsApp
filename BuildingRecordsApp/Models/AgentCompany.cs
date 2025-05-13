public class AgentCompany
{
    public int AgentCompanyId { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string RegistrationNumber { get; set; } = string.Empty;

    // Navigation property
    public ICollection<Person> Agents { get; set; } = [];
}