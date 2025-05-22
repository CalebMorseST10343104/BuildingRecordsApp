using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BuildingRecordsApp.Pages.Ownerships
{
    public class EditModel : PageModel
    {
        private readonly BuildingContext _context;
        private readonly ISelectListService _selectListService;

        public EditModel(BuildingContext context, ISelectListService selectListService)
        {
            _context = context;
            _selectListService = selectListService;
        }

        [BindProperty]
        public required Ownership Ownership { get; set; }

        public SelectList? UnitSelectList { get; set; }
        public SelectList? CompanyTrustSelectList { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            Ownership = await _context.Ownerships.FindAsync(id) ?? null!;

            if (Ownership == null)
                return NotFound();

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
                return Page();

            _context.Attach(Ownership).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OwnershipExists(Ownership.OwnershipId))
                    return NotFound();

                throw;
            }

            return RedirectToPage("/Ownerships/Index");
        }

        private bool OwnershipExists(int id)
        {
            return _context.Ownerships.Any(e => e.OwnershipId == id);
        }
    }
}
