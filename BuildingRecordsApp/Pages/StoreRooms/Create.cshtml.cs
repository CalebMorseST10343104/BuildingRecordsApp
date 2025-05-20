using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BuildingRecordsApp.Models;

namespace BuildingRecordsApp.Pages.StoreRooms
{
    public class CreateModel(BuildingContext context, ISelectListService selectListService) : PageModel
    {
        private readonly BuildingContext _context = context;
        private readonly ISelectListService _selectListService = selectListService;

        [BindProperty]
        public StoreRoom StoreRoom { get; set; } = new();
        
        public SelectList? UnitSelectList { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            UnitSelectList = await _selectListService.GetUnitSelectListAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            _context.StoreRooms.Add(StoreRoom);
            await _context.SaveChangesAsync();

            return RedirectToPage("/StoreRooms/Index");
        }
    }
}