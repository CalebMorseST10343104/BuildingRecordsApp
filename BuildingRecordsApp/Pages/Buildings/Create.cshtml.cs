using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models;
using BuildingRecordsApp.ViewModels;

namespace BuildingRecordsApp.Pages.Buildings
{
    public class CreateModel : PageModel
    {
        private readonly BuildingContext _context;

        public CreateModel(BuildingContext context)
        {
            _context = context;
        }

        [BindProperty]
        public BuildingFormViewModel ViewModel { get; set; } = new();

        public IActionResult OnGet()
        {
            ViewModel = new BuildingFormViewModel
            {
                Building = new Building()
            };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ViewModel.Building == null)
            {
                ModelState.AddModelError("ViewModel.Building", "Building details are required.");
                return Page();
            }
            if (!ModelState.IsValid)
                return Page();

            _context.Buildings.Add(ViewModel.Building);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Buildings/Index");
        }
    }
}