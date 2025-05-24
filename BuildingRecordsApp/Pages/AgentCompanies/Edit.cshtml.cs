using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models;
using BuildingRecordsApp.ViewModels;

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
        public required AgentCompanyFormViewModel ViewModel { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            ViewModel = new AgentCompanyFormViewModel
            {
                AgentCompany = await _context.AgentCompanies.FirstOrDefaultAsync(ac => ac.AgentCompanyId == id)
            };

            if (ViewModel.AgentCompany == null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ViewModel == null)
            {
                ModelState.AddModelError("ViewModel", "Agent company details are required.");
                return Page();
            }
            if (ViewModel.AgentCompany == null)
            {
                ModelState.AddModelError("ViewModel.AgentCompany", "Agent company details are required.");
                return Page();
            }
            if (!ModelState.IsValid)
                return Page();

            _context.Attach(ViewModel.AgentCompany).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AgentCompanyExists(ViewModel.AgentCompany.AgentCompanyId))
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
