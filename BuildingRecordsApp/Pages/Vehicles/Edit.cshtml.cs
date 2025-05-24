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
        public required VehicleFormViewModel ViewModel { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            ViewModel = new VehicleFormViewModel
            {
                Vehicle = await _context.Vehicles
                    .Include(v => v.Unit)
                    .FirstOrDefaultAsync(v => v.VehicleId == id),
                UnitSelectList = await _selectListService.GetUnitSelectListAsync()
            };

            if (ViewModel == null)
                return NotFound();
                
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ViewModel == null)
            {
                ModelState.AddModelError("ViewModel", "Vehicle details are required.");
                return Page();
            }
            if (ViewModel.Vehicle == null)
            {
                ModelState.AddModelError("Vehicle", "Vehicle details are required.");
                return Page();
            }
            if (ViewModel.Vehicle.UnitId == null)
            {
                ModelState.AddModelError("Vehicle.UnitId", "Unit is required.");
                return Page();
            }
            if (!ModelState.IsValid)
                return Page();

            _context.Attach(ViewModel.Vehicle).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VehicleExists(ViewModel.Vehicle.VehicleId))
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
