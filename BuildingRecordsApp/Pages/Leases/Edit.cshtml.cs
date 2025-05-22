using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BuildingRecordsApp.Models;
using Microsoft.EntityFrameworkCore;

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
        public required Lease Lease { get; set; }

        public SelectList? UnitSelectList { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            Lease = await _context.Leases
                .Include(l => l.Unit)
                .FirstOrDefaultAsync(m => m.LeaseId == id) ?? null!;

            if (Lease == null)
                return NotFound();

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

            _context.Attach(Lease).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LeaseExists(Lease!.LeaseId))
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
