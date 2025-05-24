using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models;
using BuildingRecordsApp.ViewModels;

namespace BuildingRecordsApp.Pages.Occupancies
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
        public required OccupancyFormViewModel ViewModel { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();
            ViewModel = new OccupancyFormViewModel
            {
                Occupancy = await _context.Occupancies.FindAsync(id) ?? null!,
                UnitSelectList = new SelectList(Enumerable.Empty<SelectListItem>()),
                PersonSelectList = new SelectList(Enumerable.Empty<SelectListItem>())
            };

            if (ViewModel.Occupancy == null)
                return NotFound();

            ViewModel.UnitSelectList = await _selectListService.GetUnitSelectListAsync(Enums.UsageContext.ForOccupancy);
            ViewModel.PersonSelectList = await _selectListService.GetPersonSelectListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ViewModel.Occupancy == null)
            {
                ModelState.AddModelError("ViewModel.Occupancy", "Occupancy details are required.");
                return Page();
            }
            if (ViewModel.Occupancy.UnitId == null)
            {
                ModelState.AddModelError("ViewModel.Occupancy.UnitId", "Unit is required.");
                return Page();
            }

            if (!ModelState.IsValid)
                return Page();

            _context.Attach(ViewModel.Occupancy).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OccupancyExists(ViewModel.Occupancy.OccupancyId))
                    return NotFound();

                throw;
            }

            return RedirectToPage("/Occupancies/Index");
        }

        private bool OccupancyExists(int id)
        {
            return _context.Occupancies.Any(e => e.OccupancyId == id);
        }
    }
}
