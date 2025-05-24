using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BuildingRecordsApp.Models;
using BuildingRecordsApp.ViewModels;
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
        public OccupancyFormViewModel ViewModel { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            ViewModel = new OccupancyFormViewModel
            {
                Occupancy = new Occupancy(),
                UnitSelectList = await _selectListService.GetUnitSelectListAsync(Enums.UsageContext.ForOccupancy),
                PersonSelectList = await _selectListService.GetPersonSelectListAsync()
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ViewModel.Occupancy.UnitId == null)
            {
                ModelState.AddModelError("Occupancy.UnitId", "Unit is required.");
            }

            if (!ModelState.IsValid)
                return Page();

            _context.Occupancies.Add(ViewModel.Occupancy);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Occupancies/Index");
        }
    }
}