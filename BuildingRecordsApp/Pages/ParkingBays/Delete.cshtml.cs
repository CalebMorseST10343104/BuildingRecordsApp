using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BuildingRecordsApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BuildingRecordsApp.Pages.ParkingBays
{
    public class DeleteModel : PageModel
    {
        private readonly BuildingContext _context;

        public DeleteModel(BuildingContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ParkingBay ParkingBay { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parkingBay = await _context.ParkingBays
                .Include(p => p.Unit)
                .ThenInclude(u => u!.Building)
                .FirstOrDefaultAsync(m => m.ParkingBayID == id);

            if (parkingBay == null)
            {
                return NotFound();
            }
            ParkingBay = parkingBay;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parkingBay = await _context.ParkingBays.FindAsync(id);

            if (parkingBay != null)
            {
                _context.ParkingBays.Remove(parkingBay);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
