using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models;

namespace BuildingRecordsApp.Pages.Buildings
{
    public class IndexModel : PageModel
    {
        private readonly BuildingContext _context;

        public IndexModel(BuildingContext context)
        {
            _context = context;
        }

        public List<Building> Buildings { get; set; } = new();

        public async Task OnGetAsync()
        {
            Buildings = await _context.Buildings.ToListAsync();
        }
    }
}