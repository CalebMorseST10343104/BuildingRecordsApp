using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models;

namespace BuildingRecordsApp.Pages.Ownerships
{
    public class DeleteModel : PageModel
    {
        private readonly BuildingContext _context;

        public DeleteModel(BuildingContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Ownership Ownership { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ownership = await _context.Ownerships
                .Include(o => o.Unit)
                .Include(o => o.CompanyTrust)
                .Include(o => o.Owners)
                .ThenInclude(w => w.Person)
                .FirstOrDefaultAsync(m => m.OwnershipId == id);

            if (ownership == null)
            {
                return NotFound();
            }
            Ownership = ownership;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ownership = await _context.Ownerships.FindAsync(id);

            if (ownership != null)
            {
                _context.Ownerships.Remove(ownership);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
