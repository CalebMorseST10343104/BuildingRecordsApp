using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models;

namespace BuildingRecordsApp.Pages.AgentCompanies
{
    public class IndexModel : PageModel
    {
        private readonly BuildingContext _context;

        public IndexModel(BuildingContext context)
        {
            _context = context;
        }

        public List<AgentCompany> AgentCompanies { get; set; } = new();

        public async Task OnGetAsync()
        {
            AgentCompanies = await _context.AgentCompanies.ToListAsync();
        }
    }
}