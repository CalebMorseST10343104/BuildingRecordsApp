using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Enums;
using BuildingRecordsApp.Models;
using BuildingRecordsApp.ViewModels;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BuildingRecordsApp.Pages.TagRemoteRecords
{
    public class CreateModel : PageModel
    {
        private readonly BuildingContext _context;
        private readonly ISelectListService _selectListService;

        public CreateModel(BuildingContext context, ISelectListService selectListService)
        {
            _context = context;
            _selectListService = selectListService;
        }

        [BindProperty]
        public TagRemoteRecordFormViewModel ViewModel { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            ViewModel = new TagRemoteRecordFormViewModel
            {
                TagRemoteRecord = new TagRemoteRecord(),
                UnitSelectList = await _selectListService.GetUnitSelectListAsync(UsageContext.ForTagRemoteRecord)
            };
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
            {
                Console.WriteLine("ModelState is invalid");
                return Page();
            }

            _context.TagRemoteRecords.Add(ViewModel.TagRemoteRecord);
            await _context.SaveChangesAsync();

            return RedirectToPage("/TagRemoteRecords/Index");
        }
    }
}