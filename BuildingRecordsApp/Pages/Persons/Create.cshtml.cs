using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BuildingRecordsApp.Models;
using BuildingRecordsApp.ViewModels;

namespace BuildingRecordsApp.Pages.Persons
{
    public class CreateModel : PageModel
    {
        private readonly BuildingContext _context;

        public CreateModel(BuildingContext context)
        {
            _context = context;
        }

        [BindProperty]
        public PersonFormViewModel ViewModel { get; set; } = new();

        public IActionResult OnGet()
        {
            ViewModel = new PersonFormViewModel
            {
                Person = new Person()
            };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ViewModel == null)
            {
                ModelState.AddModelError("ViewModel", "Person details are required.");
                return Page();
            }
            if (ViewModel.Person == null)
            {
                ModelState.AddModelError("ViewModel.Person", "Person details are required.");
                return Page();
            }
            if (!ModelState.IsValid)
                return Page();

            _context.Persons.Add(ViewModel.Person);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Persons/Index");
        }
    }
}