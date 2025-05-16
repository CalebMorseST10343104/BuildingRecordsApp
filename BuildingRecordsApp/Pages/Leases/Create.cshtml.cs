using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BuildingRecordsApp.Models;

namespace BuildingRecordsApp.Pages.Leases
{
    public class CreateModel(BuildingContext context, ISelectListService selectListService) : PageModel
    {
        private readonly BuildingContext _context = context;
        private readonly ISelectListService _selectListService = selectListService;


        [BindProperty]
        public Lease Lease { get; set; } = new();

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnGetAsync()
        {
            await _selectListService.GetUnitSelectListAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            _context.Leases.Add(Lease);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Leases/Index");
        }
    }
}