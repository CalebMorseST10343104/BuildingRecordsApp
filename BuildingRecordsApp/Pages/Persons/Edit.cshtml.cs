using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BuildingRecordsApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BuildingRecordsApp.Pages.Persons
{
    public class EditModel(BuildingContext context) : PageModel
    {
        private readonly BuildingContext _context = context;

        [BindProperty]
        public required Person Person { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            Person = await _context.Persons.FindAsync(id) ?? null!;

            if (Person == null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            _context.Attach(Person).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonExists(Person!.PersonId))
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