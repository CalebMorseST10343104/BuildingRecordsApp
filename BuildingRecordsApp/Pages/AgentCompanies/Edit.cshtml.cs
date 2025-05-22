using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BuildingRecordsApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BuildingRecordsApp.Pages.AgentCompanies
{
    public class EditModel : PageModel
    {
        private readonly BuildingContext _context;

        public EditModel(BuildingContext context)
        {
            _context = context;
        }

        [BindProperty]
        public required AgentCompany AgentCompany { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            AgentCompany = await _context.AgentCompanies.FindAsync(id) ?? null!;

            if (AgentCompany == null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            _context.Attach(AgentCompany).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AgentCompanyExists(AgentCompany!.AgentCompanyId))
                    return NotFound();

                throw;
            }

            return RedirectToPage("/AgentCompanies/Index");
        }

        private bool AgentCompanyExists(int id)
        {
            return _context.AgentCompanies.Any(e => e.AgentCompanyId == id);
        }
    }
}
