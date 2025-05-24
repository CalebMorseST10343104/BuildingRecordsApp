using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models;
using BuildingRecordsApp.ViewModels;

namespace BuildingRecordsApp.Pages.ParkingBays
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
        public required ParkingBay ParkingBay { get; set; }

        public SelectList? UnitSelectList { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            ParkingBay = await _context.ParkingBays
                .Include(p => p.Unit)
                .FirstOrDefaultAsync(m => m.ParkingBayID == id) ?? null!;

            if (ParkingBay == null)
                return NotFound();

            UnitSelectList = await _selectListService.GetUnitSelectListAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            _context.Attach(ParkingBay).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ParkingBayExists(ParkingBay!.ParkingBayID))
                    return NotFound();

                throw;
            }

            return RedirectToPage("/ParkingBays/Index");
        }
        
        private bool ParkingBayExists(int id)
        {
            return _context.ParkingBays.Any(e => e.ParkingBayID == id);
        }
    }
}
