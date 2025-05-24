using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models;
using BuildingRecordsApp.ViewModels;

namespace BuildingRecordsApp.Pages.ParkingBays
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
        public ParkingBayFormViewModel ViewModel { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            ViewModel = new ParkingBayFormViewModel
            {
                UnitSelectList = await _selectListService.GetUnitSelectListAsync()
            };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ViewModel == null)
            {
                ModelState.AddModelError("ViewModel", "Parking Bay details are required.");
                return Page();
            }
            if (ViewModel.ParkingBay == null)
            {
                ModelState.AddModelError("ViewModel.ParkingBay", "Parking Bay details are required.");
                return Page();
            }
            if (!ModelState.IsValid)
                return Page();

            _context.ParkingBays.Add(ViewModel.ParkingBay);
            await _context.SaveChangesAsync();

            return RedirectToPage("/ParkingBays/Index");
        }
    }
}