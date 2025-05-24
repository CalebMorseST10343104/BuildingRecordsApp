using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models;
using BuildingRecordsApp.ViewModels;
using System.Threading.Tasks;

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
        public UnitFormViewModel ViewModel { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            ViewModel = new UnitFormViewModel
            {
                Unit = _context.Units.Include(u => u.Building).FirstOrDefault(u => u.UnitId == id),
            };

            if (ViewModel == null)
                return NotFound();

            // Populate the select list for the building
            ViewModel.BuildingSelectList = await _selectListService.GetBuildingSelectListAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ViewModel == null)
            {
                ModelState.AddModelError("ViewModel", "Unit details are required.");
                return Page();
            }
            if (ViewModel.Unit == null)
            {
                ModelState.AddModelError("ViewModel.Unit", "Unit details are required.");
                return Page();
            }
            if (ViewModel.Unit.BuildingId == null)
            {
                ModelState.AddModelError("ViewModel.Unit.BuildingId", "Building is required.");
                return Page();
            }

            if (!ModelState.IsValid)
                return Page();

            _context.Attach(ViewModel.Unit).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UnitExists(ViewModel.Unit.UnitId))
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
