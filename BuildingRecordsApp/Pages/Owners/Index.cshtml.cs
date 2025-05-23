using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models;

namespace BuildingRecordsApp.Pages.Owners
{
    public class IndexModel : PageModel
    {
        private readonly BuildingContext _context;

        public IndexModel(BuildingContext context)
        {
            _context = context;
        }

        public List<Owner> Owners { get; set; } = [];

        public async Task OnGetAsync()
        {
            Owners = await _context.Owners
            .Include(o => o.Person)
            .Include(o => o.Ownership)
            .ThenInclude(u => u!.Unit)
            .ThenInclude(u => u!.Building)
            .ToListAsync();
        }
    }
}