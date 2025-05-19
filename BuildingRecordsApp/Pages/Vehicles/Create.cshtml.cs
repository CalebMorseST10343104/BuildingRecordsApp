using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models;

namespace BuildingRecordsApp.Pages.Vehicles
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
        public Vehicle Vehicle { get; set; } = new();

        public SelectList? UnitSelectList { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            UnitSelectList = await _selectListService.GetUnitSelectListAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            _context.Vehicles.Add(Vehicle);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Vehicles/Index");
        }
    }
}