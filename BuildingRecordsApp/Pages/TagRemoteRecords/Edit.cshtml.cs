using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BuildingRecordsApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BuildingRecordsApp.Pages.TagRemoteRecords
{
    public class EditModel : PageModel
    {
        private readonly BuildingContext _context;
        private readonly ISelectListService _selectListService;

        public EditModel(BuildingContext context, ISelectListService selectListService)
        {
            _context = context;
            _selectListService = selectListService;
        }

        [BindProperty]
        public required TagRemoteRecord TagRemoteRecord { get; set; }

        public SelectList? UnitSelectList { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            TagRemoteRecord = await _context.TagRemoteRecords
                .Include(t => t.Unit)
                .FirstOrDefaultAsync(m => m.TagRemoteRecordId == id) ?? null!;

            if (TagRemoteRecord == null)
                return NotFound();

            UnitSelectList = await _selectListService.GetUnitSelectListAsync(Enums.UsageContext.ForTagRemoteRecord);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (TagRemoteRecord.UnitId == 0)
            {
                ModelState.AddModelError("TagRemoteRecord.UnitId", "Unit is required.");
            }

            if (!ModelState.IsValid)
                return Page();

            _context.Attach(TagRemoteRecord).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TagRemoteRecordExists(TagRemoteRecord!.TagRemoteRecordId))
                    return NotFound();

                throw;
            }

            return RedirectToPage("/TagRemoteRecords/Index");
        }

        private bool TagRemoteRecordExists(int id)
        {
            return _context.TagRemoteRecords.Any(e => e.TagRemoteRecordId == id);
        }
    }
}
