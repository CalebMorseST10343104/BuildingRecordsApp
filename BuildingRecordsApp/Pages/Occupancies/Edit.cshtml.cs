using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BuildingRecordsApp.Models;
using Microsoft.EntityFrameworkCore;

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
        public required Occupancy Occupancy { get; set; }

        public SelectList? UnitSelectList { get; set; }
        public SelectList? PersonSelectList { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            Occupancy = await _context.Occupancies.FindAsync(id) ?? null!;

            if (Occupancy == null)
                return NotFound();

            UnitSelectList = await _selectListService.GetUnitSelectListAsync(Enums.UsageContext.ForOccupancy);
            PersonSelectList = await _selectListService.GetPersonSelectListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Occupancy.UnitId == 0)
            {
                ModelState.AddModelError("Occupancy.UnitId", "Unit is required.");
            }

            if (!ModelState.IsValid)
                return Page();

            _context.Attach(Occupancy).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OccupancyExists(Occupancy.OccupancyId))
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
