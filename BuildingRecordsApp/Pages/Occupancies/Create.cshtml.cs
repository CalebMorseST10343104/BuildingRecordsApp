using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BuildingRecordsApp.Models;
using BuildingRecordsApp.Services;

namespace BuildingRecordsApp.Pages.Occupancies
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
        public Occupancy Occupancy { get; set; } = new();

        public SelectList? UnitSelectList { get; set; }
        public SelectList? PersonSelectList { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            UnitSelectList = await _selectListService.GetUnitSelectListAsync();
            PersonSelectList = await _selectListService.GetPersonSelectListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            _context.Occupancies.Add(Occupancy);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Occupancies/Index");
        }
    }
}