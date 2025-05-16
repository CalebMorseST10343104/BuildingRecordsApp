using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BuildingRecordsApp.Models;

namespace BuildingRecordsApp.Pages.Owners
{
    public class CreateModel(BuildingContext context, ISelectListService selectListService) : PageModel
    {
        private readonly BuildingContext _context = context;
        private readonly ISelectListService _selectListService = selectListService;

        [BindProperty]
        public Owner Owner { get; set; } = new();

        public SelectList? OwnershipSelectList { get; set; }
        public SelectList? PersonSelectList { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnGetAsync()
        {
            OwnershipSelectList = await _selectListService.GetOwnershipSelectListAsync();
            PersonSelectList = await _selectListService.GetPersonSelectListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            _context.Owners.Add(Owner);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Owner/Index");
        }
    }
}