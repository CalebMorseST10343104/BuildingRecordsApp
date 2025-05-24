using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models;
using BuildingRecordsApp.ViewModels;

namespace BuildingRecordsApp.Pages.Persons
{
    public class EditModel : PageModel
    {
        private readonly BuildingContext _context;

        public EditModel(BuildingContext context)
        {
            _context = context;
        }

        [BindProperty]
        public required PersonFormViewModel ViewModel { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            ViewModel = new PersonFormViewModel
            {
                Person = await _context.Persons.FirstOrDefaultAsync(p => p.PersonId == id)
            };

            if (ViewModel == null)
                return NotFound();

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

            _context.Attach(ViewModel.Person).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonExists(ViewModel.Person.PersonId))
                    return NotFound();

                throw;
            }
            
            return RedirectToPage("/Persons/Index");
        }

        private bool PersonExists(int id)
        {
            return _context.Persons.Any(e => e.PersonId == id);
        }
    }
}