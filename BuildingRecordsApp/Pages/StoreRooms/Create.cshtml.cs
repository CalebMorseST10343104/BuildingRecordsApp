using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BuildingRecordsApp.Models;
using BuildingRecordsApp.ViewModels;

namespace BuildingRecordsApp.Pages.StoreRooms
{
    public class CreateModel(BuildingContext context, ISelectListService selectListService) : PageModel
    {
        private readonly BuildingContext _context = context;
        private readonly ISelectListService _selectListService = selectListService;

        [BindProperty]
        public StoreRoomFormViewModel ViewModel { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            ViewModel = new StoreRoomFormViewModel
            {
                StoreRoom = new StoreRoom(),
                UnitSelectList = await _selectListService.GetUnitSelectListAsync()
            };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ViewModel == null)
            {
                ModelState.AddModelError("ViewModel", "Store Room details are required.");
                return Page();
            }   
            if (ViewModel.StoreRoom == null)
            {
                ModelState.AddModelError("ViewModel.StoreRoom", "Store Room details are required.");
                return Page();
            }
            if (!ModelState.IsValid)
                return Page();

            _context.StoreRooms.Add(ViewModel.StoreRoom);
            await _context.SaveChangesAsync();

            return RedirectToPage("/StoreRooms/Index");
        }
    }
}