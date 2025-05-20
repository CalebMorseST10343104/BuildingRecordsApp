using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models;

namespace BuildingRecordsApp.Pages.ParkingBays
{
    public class IndexModel : PageModel
    {
        private readonly BuildingContext _context;

        public IndexModel(BuildingContext context)
        {
            _context = context;
        }

        public List<ParkingBay> ParkingBays { get; set; } = new();

        public async Task OnGetAsync()
        {
            ParkingBays = await _context.ParkingBays
            .Include(p => p.Unit)
            .ThenInclude(u => u!.Building)
            .ToListAsync();
        }
    }
}