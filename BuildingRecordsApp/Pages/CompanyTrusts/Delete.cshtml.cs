using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models;

namespace BuildingRecordsApp.Pages.CompanyTrusts
{
    public class DeleteModel : PageModel
    {
        private readonly BuildingContext _context;

        public DeleteModel(BuildingContext context)
        {
            _context = context;
        }

        [BindProperty]
        public CompanyTrust CompanyTrust { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyTrust = await _context.CompanyTrusts
                .Include(c => c.Ownerships)
                .ThenInclude(o => o.Unit)
                .ThenInclude(u => u!.Building)
                .FirstOrDefaultAsync(m => m.CompanyTrustId == id);

            if (companyTrust == null)
            {
                return NotFound();
            }
            CompanyTrust = companyTrust;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyTrust = await _context.CompanyTrusts.FindAsync(id);

            if (companyTrust != null)
            {
                _context.CompanyTrusts.Remove(companyTrust);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
