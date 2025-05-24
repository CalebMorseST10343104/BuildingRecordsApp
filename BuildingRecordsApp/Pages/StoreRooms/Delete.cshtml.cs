using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BuildingRecordsApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BuildingRecordsApp.Pages.StoreRooms
{
    public class DeleteModel : PageModel
    {
        private readonly BuildingContext _context;

        public DeleteModel(BuildingContext context)
        {
            _context = context;
        }

        [BindProperty]
        public StoreRoom StoreRoom { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var storeRoom = await _context.StoreRooms
                .Include(s => s.Unit)
                .ThenInclude(u => u!.Building)
                .FirstOrDefaultAsync(m => m.StoreRoomId == id);

            if (storeRoom == null)
            {
                return NotFound();
            }
            StoreRoom = storeRoom;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var storeRoom = await _context.StoreRooms.FindAsync(id);

            if (storeRoom != null)
            {
                _context.StoreRooms.Remove(storeRoom);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
