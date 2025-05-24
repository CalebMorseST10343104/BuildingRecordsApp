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
        public required ParkingBayFormViewModel ViewModel { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            ViewModel = new ParkingBayFormViewModel
            {
                ParkingBay = await _context.ParkingBays
                    .Include(pb => pb.Unit)
                    .FirstOrDefaultAsync(pb => pb.ParkingBayID == id),
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
                ModelState.AddModelError("ViewModel", "Parking Bay details are required.");
                return Page();
            }
            if (ViewModel.ParkingBay == null)
            {
                ModelState.AddModelError("ViewModel.ParkingBay", "Parking Bay details are required.");
                return Page();
            }
            if (!ModelState.IsValid)
                return Page();

            _context.Attach(ViewModel.ParkingBay).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ParkingBayExists(ViewModel.ParkingBay.ParkingBayID))
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
