using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BuildingRecordsApp.Models;

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
        public Lease Lease { get; set; } = new();

        public SelectList? UnitSelectList { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            UnitSelectList = await _selectListService.GetUnitSelectListAsync(Enums.UsageContext.ForLease);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Lease.UnitId == 0)
            {
                ModelState.AddModelError("Lease.UnitId", "Unit is required.");
            }
            if (!ModelState.IsValid)
                return Page();

            _context.Leases.Add(Lease);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Leases/Index");
        }
    }
}