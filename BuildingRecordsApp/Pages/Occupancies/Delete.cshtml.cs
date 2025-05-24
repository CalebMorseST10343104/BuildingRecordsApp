using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models;

namespace BuildingRecordsApp.Pages.Occupancies
{
    public class DeleteModel : PageModel
    {
        private readonly BuildingContext _context;

        public DeleteModel(BuildingContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Occupancy Occupancy { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var occupancy = await _context.Occupancies
                .Include(o => o.Unit)
                .ThenInclude(u => u!.Building)
                .Include(o => o.Occupant)
                .FirstOrDefaultAsync(m => m.OccupancyId == id);

            if (occupancy == null)
            {
                return NotFound();
            }
            Occupancy = occupancy;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var occupancy = await _context.Occupancies.FindAsync(id);

            if (occupancy != null)
            {
                _context.Occupancies.Remove(occupancy);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
