using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models;
using BuildingRecordsApp.ViewModels;

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
        public required OwnershipFormViewModel ViewModel { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            ViewModel = new OwnershipFormViewModel
            {
                Ownership = await _context.Ownerships
                    .Include(o => o.Unit)
                    .Include(o => o.CompanyTrust)
                    .FirstOrDefaultAsync(m => m.OwnershipId == id) ?? null!,
                UnitSelectList = await _selectListService.GetUnitSelectListAsync(Enums.UsageContext.ForOwnership),
                CompanyTrustSelectList = await _selectListService.GetCompanyTrustSelectListAsync()
            };
            
            if (ViewModel == null)
                return NotFound();

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
                ModelState.AddModelError("Ownership.UnitId", "Unit is required.");
                return Page();
            }

            if (!ModelState.IsValid)
                return Page();

            _context.Attach(ViewModel.Ownership).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OwnershipExists(ViewModel.Ownership.OwnershipId))
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
