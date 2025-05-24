using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BuildingRecordsApp.Models;
using BuildingRecordsApp.ViewModels;

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
        public UnitFormViewModel ViewModel { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            ViewModel = new UnitFormViewModel
            {
                Unit = new Unit(),
                BuildingSelectList = await _selectListService.GetBuildingSelectListAsync()
            };
            return Page();
        }
        
        public async Task<IActionResult> OnPostAsync()
        {
            if (ViewModel == null)
            {
                ModelState.AddModelError("ViewModel", "Unit details are required.");
                return Page();
            }
            if (ViewModel.Unit == null)
            {
                ModelState.AddModelError("ViewModel.Unit", "Unit details are required.");
                return Page();
            }
            if (ViewModel.Unit.BuildingId == null)
            {
                ModelState.AddModelError("ViewModel.Unit.BuildingId", "Building is required.");
                return Page();
            }

            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState is invalid");
                return Page();
            }

            _context.Units.Add(ViewModel.Unit);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Units/Index");
        }
    }
}
