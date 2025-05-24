using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BuildingRecordsApp.Models;
using BuildingRecordsApp.ViewModels;

namespace BuildingRecordsApp.Pages.Ownerships
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
        public OwnershipFormViewModel ViewModel { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            ViewModel = new OwnershipFormViewModel
            {
                Ownership = new Ownership(),
                UnitSelectList = await _selectListService.GetUnitSelectListAsync(Enums.UsageContext.ForOwnership),
                CompanyTrustSelectList = await _selectListService.GetCompanyTrustSelectListAsync()
            };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ViewModel == null)
            {
                ModelState.AddModelError("ViewModel", "Ownership details are required.");
                return Page();
            }
            if (ViewModel.Ownership == null)
            {
                ModelState.AddModelError("ViewModel.Ownership", "Ownership details are required.");
                return Page();
            }
            if (ViewModel.Ownership.UnitId == null)
            {
                ModelState.AddModelError("ViewModel.Ownership.UnitId", "Unit is required.");
                return Page();
            }

            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState is invalid");
                return Page();
            }
            
            _context.Ownerships.Add(ViewModel.Ownership);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Ownerships/Index");
        }
    }
}