using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models;

namespace BuildingRecordsApp.Pages.Owners
{
    public class DeleteModel : PageModel
    {
        private readonly BuildingContext _context;

        public DeleteModel(BuildingContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Owner Owner { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Owner = await _context.Owners
                .Include(o => o.Person)
                .Include(o => o.Ownership)
                .ThenInclude(os => os!.Unit)
                .ThenInclude(u => u!.Building)
                .FirstOrDefaultAsync(m => m.OwnerId == id);

            if (Owner == null)
            {
                return NotFound();
            }
            this.Owner = Owner;
            return Page();
        }
        
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Owner = await _context.Owners.FindAsync(id);

            if (Owner != null)
            {
                _context.Owners.Remove(Owner);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
