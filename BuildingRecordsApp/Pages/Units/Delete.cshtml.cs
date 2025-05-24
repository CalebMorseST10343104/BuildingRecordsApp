using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models;

namespace BuildingRecordsApp.Pages.Units
{
    public class DeleteModel : PageModel
    {
        private readonly BuildingContext _context;

        public DeleteModel(BuildingContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Unit Unit { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var unit = await _context.Units
                .Include(u => u.Building)
                .Include(u => u.PrimaryContactPerson)
                .FirstOrDefaultAsync(m => m.UnitId == id);

            if (unit == null)
            {
                return NotFound();
            }
            Unit = unit;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var unit = await _context.Units.FindAsync(id);

            if (unit != null)
            {
                _context.Units.Remove(unit);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
