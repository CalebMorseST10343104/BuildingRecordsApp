using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models;
using BuildingRecordsApp.ViewModels;

namespace BuildingRecordsApp.Pages.CompanyTrusts
{
    public class EditModel : PageModel
    {
        private readonly BuildingContext _context;

        public EditModel(BuildingContext context)
        {
            _context = context;
        }
        [BindProperty]
        public required CompanyTrustFormViewModel ViewModel { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            ViewModel = new CompanyTrustFormViewModel
            {
                CompanyTrust = await _context.CompanyTrusts.FindAsync(id) ?? null!
            };

            if (ViewModel.CompanyTrust == null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {   
            if (ViewModel == null)
            {
                ModelState.AddModelError("ViewModel", "Company Trust details are required.");
                return Page();
            }
            if (ViewModel.CompanyTrust == null)
            {
                ModelState.AddModelError("ViewModel.CompanyTrust", "Company Trust details are required.");
                return Page();
            }
            if (!ModelState.IsValid)
                return Page();

            _context.Attach(ViewModel.CompanyTrust).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompanyTrustExists(ViewModel.CompanyTrust.CompanyTrustId))
                    return NotFound();

                throw;
            }

            return RedirectToPage("/CompanyTrusts/Index");
        }

        private bool CompanyTrustExists(int id)
        {
            return _context.CompanyTrusts.Any(e => e.CompanyTrustId == id);
        }
    }
}
