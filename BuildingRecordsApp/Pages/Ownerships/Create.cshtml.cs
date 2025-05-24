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
        public Ownership Ownership { get; set; } = new();

        public SelectList? UnitSelectList { get; set; }
        public SelectList? CompanyTrustSelectList { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            UnitSelectList = await _selectListService.GetUnitSelectListAsync(Enums.UsageContext.ForOwnership);
            CompanyTrustSelectList = await _selectListService.GetCompanyTrustSelectListAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {

            if (Ownership.UnitId == 0)
            {
                ModelState.AddModelError("Ownership.UnitId", "Unit is required.");
            }

            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState is invalid");
                return Page();
            }
            
            _context.Ownerships.Add(Ownership);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Ownerships/Index");
        }
    }
}