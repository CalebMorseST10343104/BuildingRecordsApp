using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BuildingRecordsApp.Pages.Units
{
    public class CreateModel(BuildingContext context) : PageModel
    {
        private readonly BuildingContext _context = context;

        [BindProperty]
        public Unit Unit { get; set; } = new();

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            _context.Units.Add(Unit);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Units/Index");
        }
    }
}
