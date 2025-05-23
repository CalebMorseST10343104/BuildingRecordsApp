using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models;
using BuildingRecordsApp.ViewModels;

namespace BuildingRecordsApp.Pages.Leases
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
        public required LeaseFormViewModel ViewModel { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            ViewModel = new LeaseFormViewModel
            {
                Lease = await _context.Leases
                    .Include(l => l.Unit)
                    .FirstOrDefaultAsync(m => m.LeaseId == id) ?? null!,
                UnitSelectList = await _selectListService.GetUnitSelectListAsync(Enums.UsageContext.ForLease)
            };

            if (ViewModel.Lease == null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ViewModel.Lease == null)
            {
                ModelState.AddModelError("ViewModel.Lease", "Lease details are required.");
                return Page();
            }
            if (ViewModel.Lease.UnitId == null)
            {
                ModelState.AddModelError("ViewModel.Lease.UnitId", "Unit is required.");
                return Page();
            }

            if (!ModelState.IsValid)
                return Page();

            _context.Attach(ViewModel.Lease).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LeaseExists(ViewModel!.Lease.LeaseId))
                    return NotFound();

                throw;
            }

            return RedirectToPage("/Leases/Index");
        }

        private bool LeaseExists(int id)
        {
            return _context.Leases.Any(e => e.LeaseId == id);
        }
    }
}
