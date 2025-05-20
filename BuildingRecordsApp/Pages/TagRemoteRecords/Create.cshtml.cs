using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BuildingRecordsApp.Models;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Enums;

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
        public TagRemoteRecord TagRemoteRecord { get; set; } = new();

        public SelectList? UnitSelectList { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            UnitSelectList = await _selectListService.GetUnitSelectListAsync(UsageContext.ForTagRemoteRecord);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState is invalid");
                return Page();
            }

            _context.TagRemoteRecords.Add(TagRemoteRecord);
            await _context.SaveChangesAsync();

            return RedirectToPage("/TagRemoteRecords/Index");
        }
    }
}