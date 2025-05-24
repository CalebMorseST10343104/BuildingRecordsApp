using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models;
using BuildingRecordsApp.ViewModels;

namespace BuildingRecordsApp.Pages.Buildings
{
    public class EditModel : PageModel
    {
        private readonly BuildingContext _context;

        public EditModel(BuildingContext context)
        {
            _context = context;
        }

        [BindProperty]
        public required BuildingFormViewModel ViewModel { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            ViewModel = new BuildingFormViewModel
            {
                Building = await _context.Buildings.FirstOrDefaultAsync(m => m.BuildingId == id)
            };

            if (ViewModel.Building == null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ViewModel == null)
            {
                ModelState.AddModelError("ViewModel", "Building details are required.");
                return Page();
            }
            if (ViewModel.Building == null)
            {
                ModelState.AddModelError("ViewModel.Building", "Building details are required.");
                return Page();
            }
            if (!ModelState.IsValid)
                return Page();

            _context.Attach(ViewModel.Building).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BuildingExists(ViewModel.Building.BuildingId))
                    return NotFound();

                throw;
            }

            return RedirectToPage("/Buildings/Index");
        }
        
        private bool BuildingExists(int id)
        {
            return _context.Buildings.Any(e => e.BuildingId == id);
        }
    }
}
