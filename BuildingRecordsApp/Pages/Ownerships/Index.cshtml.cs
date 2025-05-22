using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models;

namespace BuildingRecordsApp.Pages.Ownerships
{
    public class IndexModel : PageModel
    {
        private readonly BuildingContext _context;

        public IndexModel(BuildingContext context)
        {
            _context = context;
        }

        public List<Ownership> Ownerships { get; set; } = [];

        public async Task OnGetAsync()
        {
            Ownerships = await _context.Ownerships
            .Include(o => o.CompanyTrust)
            .Include(o => o.Unit)
            .ThenInclude(u => u!.Building)
            .ToListAsync();
        }
    }
}