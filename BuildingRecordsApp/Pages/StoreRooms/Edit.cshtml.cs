using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models;
using BuildingRecordsApp.ViewModels;

namespace BuildingRecordsApp.Pages.StoreRooms
{
    public class EditModel : PageModel
    {
        private readonly BuildingContext _context;
        private readonly ISelectListService _selectListService;

        public EditModel(BuildingContext context, ISelectListService selectListService)
        {
            _context = context;
            _selectListService = selectListService;
        }

        [BindProperty]
        public required StoreRoom StoreRoom { get; set; }

        public SelectList? UnitSelectList { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            StoreRoom = await _context.StoreRooms
                .Include(s => s.Unit)
                .FirstOrDefaultAsync(m => m.StoreRoomId == id) ?? null!;

            if (StoreRoom == null)
                return NotFound();

            UnitSelectList = await _selectListService.GetUnitSelectListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            _context.Attach(StoreRoom).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StoreRoomExists(StoreRoom!.StoreRoomId))
                    return NotFound();

                throw;
            }

            return RedirectToPage("/StoreRooms/Index");
        }

        private bool StoreRoomExists(int id)
        {
            return _context.StoreRooms.Any(e => e.StoreRoomId == id);
        }
    }
}
