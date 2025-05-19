using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BuildingRecordsApp.Models;

namespace BuildingRecordsApp.Pages.Units
{
    public class CreateModel : PageModel
    {
        private readonly BuildingContext _context;
        private readonly ISelectListService _selectListService;

        public CreateModel(BuildingContext context, ISelectListService selectListService)
        {
            _context = context;
            _selectListService = selectListService;
        }

        [BindProperty]
        public Unit Unit { get; set; } = new();

        public SelectList? BuildingSelectList { get; set; }

        public IActionResult OnGetAsync()
        {
            BuildingSelectList = _selectListService.GetBuildingSelectListAsync().Result;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState is invalid");
                return Page();
            }

            _context.Units.Add(Unit);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Units/Index");
        }
    }
}
