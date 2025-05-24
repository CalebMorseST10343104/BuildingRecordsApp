using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BuildingRecordsApp.Models;
using BuildingRecordsApp.ViewModels;

namespace BuildingRecordsApp.Pages.CompanyTrusts
{
    public class CreateModel : PageModel
    {
        private readonly BuildingContext _context;

        public CreateModel(BuildingContext context)
        {
            _context = context;
        }

        [BindProperty]
        public CompanyTrustFormViewModel ViewModel { get; set; } = new();

        public IActionResult OnGet()
        {
            ViewModel = new CompanyTrustFormViewModel
            {
                CompanyTrust = new CompanyTrust()
            };
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (ViewModel.CompanyTrust == null)
            {
                ModelState.AddModelError("ViewModel.CompanyTrust", "Company Trust details are required.");
                return Page();
            }
            if (!ModelState.IsValid)
                return Page();

            _context.CompanyTrusts.Add(ViewModel.CompanyTrust);
            await _context.SaveChangesAsync();

            return RedirectToPage("/CompanyTrusts/Index");
        }
    }
}