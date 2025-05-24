using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models;
using BuildingRecordsApp.ViewModels;

namespace BuildingRecordsApp.Pages.Units
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
        public Unit Unit { get; set; } = new();

        public SelectList? BuildingSelectList { get; set; }

        public IActionResult OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            Unit = _context.Units.Find(id) ?? null!;

            if (Unit == null)
                return NotFound();

            // Populate the select list for the building
            BuildingSelectList = _selectListService.GetBuildingSelectListAsync().Result;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Unit.BuildingId == 0)
            {
                ModelState.AddModelError("Unit.BuildingId", "Building is required.");
                BuildingSelectList = await _selectListService.GetBuildingSelectListAsync();
                return Page();
            }

            if (!ModelState.IsValid)
                return Page();

            _context.Attach(Unit).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UnitExists(Unit.UnitId))
                    return NotFound();

                throw;
            }
            return RedirectToPage("/Units/Index");
        }

        private bool UnitExists(int id)
        {
            return _context.Units.Any(e => e.UnitId == id);
        }
    }
}
