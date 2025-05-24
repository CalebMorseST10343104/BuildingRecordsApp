using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models;
using BuildingRecordsApp.ViewModels;

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
        public VehicleFormViewModel ViewModel { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            ViewModel = new VehicleFormViewModel
            {
                Vehicle = new Vehicle(),
                UnitSelectList = await _selectListService.GetUnitSelectListAsync()
            };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ViewModel == null)
            {
                ModelState.AddModelError("ViewModel", "Vehicle details are required.");
                return Page();
            }
            if (ViewModel.Vehicle == null)
            {
                ModelState.AddModelError("ViewModel.Vehicle", "Vehicle details are required.");
                return Page();
            }
            if (ViewModel.Vehicle.UnitId == null)
            {
                ModelState.AddModelError("ViewModel.Vehicle.UnitId", "Unit is required.");
                return Page();
            }
            if (!ModelState.IsValid)
                return Page();

            _context.Vehicles.Add(ViewModel.Vehicle);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Vehicles/Index");
        }
    }
}