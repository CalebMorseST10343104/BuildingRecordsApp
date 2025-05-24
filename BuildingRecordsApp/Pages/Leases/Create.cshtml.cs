using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BuildingRecordsApp.Models;
using BuildingRecordsApp.ViewModels;

namespace BuildingRecordsApp.Pages.Leases
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
        public LeaseFormViewModel ViewModel { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            ViewModel = new LeaseFormViewModel
            {
                Lease = new Lease(),
                UnitSelectList = await _selectListService.GetUnitSelectListAsync(Enums.UsageContext.ForLease)
            };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ViewModel.Lease.Unit == null)
            {
                ModelState.AddModelError("Lease.UnitId", "Unit is required.");
            }
            if (!ModelState.IsValid)
                return Page();

            _context.Leases.Add(ViewModel.Lease);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Leases/Index");
        }
    }
}