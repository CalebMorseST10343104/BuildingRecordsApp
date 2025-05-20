using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models;

namespace BuildingRecordsApp.Pages.Vehicles
{
    public class IndexModel : PageModel
    {
        private readonly BuildingContext _context;

        public IndexModel(BuildingContext context)
        {
            _context = context;
        }

        public List<Vehicle> Vehicles { get; set; } = new();

        public async Task OnGetAsync()
        {
            Vehicles = await _context.Vehicles
            .Include(v => v.Unit)
            .ToListAsync();
        }
    }
}