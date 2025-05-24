using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BuildingRecordsApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BuildingRecordsApp.Pages.TagRemoteRecords
{
    public class DeleteModel : PageModel
    {
        private readonly BuildingContext _context;

        public DeleteModel(BuildingContext context)
        {
            _context = context;
        }

        [BindProperty]
        public TagRemoteRecord TagRemoteRecord { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tagRemoteRecord = await _context.TagRemoteRecords
                .Include(t => t.Unit)
                .ThenInclude(u => u!.Building)
                .FirstOrDefaultAsync(m => m.TagRemoteRecordId == id);

            if (tagRemoteRecord == null)
            {
                return NotFound();
            }
            TagRemoteRecord = tagRemoteRecord;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tagRemoteRecord = await _context.TagRemoteRecords.FindAsync(id);

            if (tagRemoteRecord != null)
            {
                _context.TagRemoteRecords.Remove(tagRemoteRecord);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
