using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models;

namespace BuildingRecordsApp.Pages.StoreRooms
{
    public class IndexModel : PageModel
    {
        private readonly BuildingContext _context;

        public IndexModel(BuildingContext context)
        {
            _context = context;
        }

        public List<StoreRoom> StoreRooms { get; set; } = new();

        public async Task OnGetAsync()
        {
            StoreRooms = await _context.StoreRooms.ToListAsync();
        }
    }
}