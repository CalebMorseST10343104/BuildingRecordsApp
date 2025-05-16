using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models;

namespace BuildingRecordsApp.Pages.Occupancies
{
    public class IndexModel : PageModel
    {
        private readonly BuildingContext _context;

        public IndexModel(BuildingContext context)
        {
            _context = context;
        }

        public List<Occupancy> Occupancies { get; set; } = new();

        public async Task OnGetAsync()
        {
            Occupancies = await _context.Occupancies
                .Include(o => o.Unit)
                .Include(o => o.Occupant)
                .ToListAsync();
        }
    }
}