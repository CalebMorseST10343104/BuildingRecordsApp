using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models;
using BuildingRecordsApp.ViewModels;

namespace BuildingRecordsApp.Pages.Vehicles
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
        public required Vehicle Vehicle { get; set; }

        public SelectList? UnitSelectList { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            Vehicle = await _context.Vehicles
                .Include(v => v.Unit)
                .FirstOrDefaultAsync(m => m.VehicleId == id) ?? null!;

            if (Vehicle == null)
                return NotFound();

            UnitSelectList = await _selectListService.GetUnitSelectListAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Vehicle.UnitId == 0)
            {
                ModelState.AddModelError("Vehicle.UnitId", "Unit is required.");
            }
            if (!ModelState.IsValid)
                return Page();

            _context.Attach(Vehicle).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VehicleExists(Vehicle!.VehicleId))
                    return NotFound();

                throw;
            }

            return RedirectToPage("/Vehicles/Index");
        }

        private bool VehicleExists(int id)
        {
            return _context.Vehicles.Any(e => e.VehicleId == id);
        }
    }
}
