using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models;
using BuildingRecordsApp.ViewModels;

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
        public required TagRemoteRecordFormViewModel ViewModel { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            ViewModel = new TagRemoteRecordFormViewModel
            {
                TagRemoteRecord = await _context.TagRemoteRecords
                    .Include(tr => tr.Unit)
                    .FirstOrDefaultAsync(tr => tr.TagRemoteRecordId == id)
            };

            if (ViewModel == null)
                return NotFound();

            ViewModel.UnitSelectList = await _selectListService.GetUnitSelectListAsync(Enums.UsageContext.ForTagRemoteRecord);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ViewModel == null)
            {
                ModelState.AddModelError("ViewModel", "Tag Remote Record details are required.");
                return Page();
            }
            if (ViewModel.TagRemoteRecord == null)
            {
                ModelState.AddModelError("ViewModel.TagRemoteRecord", "Tag Remote Record details are required.");
                return Page();
            }
            if (ViewModel.TagRemoteRecord.UnitId == null)
            {
                ModelState.AddModelError("ViewModel.TagRemoteRecord.UnitId", "Unit is required.");
                return Page();
            }

            if (!ModelState.IsValid)
                return Page();

            _context.Attach(ViewModel.TagRemoteRecord).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TagRemoteRecordExists(ViewModel.TagRemoteRecord.TagRemoteRecordId))
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
