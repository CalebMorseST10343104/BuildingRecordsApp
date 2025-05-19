using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models;

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
        public Building Building { get; set; } = new();

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            _context.Buildings.Add(Building);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Buildings/Index");
        }
    }
}