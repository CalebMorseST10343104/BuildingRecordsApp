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
        public required StoreRoomFormViewModel ViewModel { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            ViewModel = new StoreRoomFormViewModel
            {
                StoreRoom = await _context.StoreRooms
                    .Include(sr => sr.Unit)
                    .FirstOrDefaultAsync(sr => sr.StoreRoomId == id),
                UnitSelectList = await _selectListService.GetUnitSelectListAsync()
            };

            if (ViewModel.StoreRoom == null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ViewModel == null)
            {
                ModelState.AddModelError("ViewModel", "Store Room details are required.");
                return Page();
            }
            if (ViewModel.StoreRoom == null)
            {
                ModelState.AddModelError("ViewModel.StoreRoom", "Store Room details are required.");
                return Page();
            }
            if (!ModelState.IsValid)
                return Page();

            _context.Attach(ViewModel.StoreRoom).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StoreRoomExists(ViewModel.StoreRoom.StoreRoomId))
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
