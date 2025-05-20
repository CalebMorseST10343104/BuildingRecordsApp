using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models;

namespace BuildingRecordsApp.Pages.Leases
{
    public class IndexModel : PageModel
    {
        private readonly BuildingContext _context;

        public IndexModel(BuildingContext context)
        {
            _context = context;
        }

        public List<Lease> Leases { get; set; } = new();

        public async Task OnGetAsync()
        {
            // Load the leases from the database
            Leases = await _context.Leases
            .Include(l => l.Unit)
            .ThenInclude(u => u!.Building)
            .ToListAsync();
        }
    }
}