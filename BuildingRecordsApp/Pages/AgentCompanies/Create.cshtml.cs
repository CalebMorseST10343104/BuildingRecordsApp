using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BuildingRecordsApp.Models;
using BuildingRecordsApp.ViewModels;

namespace BuildingRecordsApp.Pages.AgentCompanies
{
    public class CreateModel : PageModel
    {
        private readonly BuildingContext _context;

        public CreateModel(BuildingContext context)
        {
            _context = context;
        }

        [BindProperty]
        public AgentCompanyFormViewModel ViewModel { get; set; } = new();

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ViewModel.AgentCompany == null)
            {
                ModelState.AddModelError("ViewModel.AgentCompany", "Agent company details are required.");
                return Page();
            }
                
            if (!ModelState.IsValid)
                return Page();

            _context.AgentCompanies.Add(ViewModel.AgentCompany);
            await _context.SaveChangesAsync();

            return RedirectToPage("/AgentCompanies/Index");
        }
    }
}