using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models;

namespace BuildingRecordsApp.Pages.Leases
{
    public class DeleteModel : PageModel
    {
        private readonly BuildingContext _context;

        public DeleteModel(BuildingContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Lease Lease { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lease = await _context.Leases
                .Include(l => l.Unit)
                .ThenInclude(u => u!.Building)
                .FirstOrDefaultAsync(m => m.LeaseId == id);

            if (lease == null)
            {
                return NotFound();
            }
            Lease = lease;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lease = await _context.Leases.FindAsync(id);

            if (lease != null)
            {
                _context.Leases.Remove(lease);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
