using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using BuildingRecordsApp.Models.DisplayViewModels;
using BuildingRecordsApp.Models.ItemViewModels;

namespace BuildingRecordsApp.Pages.StoreRooms
{
    public class DeleteModel : PageModel
    {
        private readonly BuildingContext _context;
        private readonly IMapper _mapper;

        public DeleteModel(BuildingContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [BindProperty]
        public DisplayViewModel<StoreRoomItemViewModel> ViewModel { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            var storeRoom = await _context.StoreRooms
                .Include(s => s.Unit)
                .ThenInclude(u => u!.Building)
                .FirstOrDefaultAsync(m => m.StoreRoomId == id);

            if (storeRoom == null)
                return NotFound();
            
            ViewModel = new DisplayViewModel<StoreRoomItemViewModel>
            {
                Entries = [_mapper.Map<StoreRoomItemViewModel>(storeRoom)],
                IdsToDisplay = [storeRoom.StoreRoomId],
                DisplayMode = Enums.DisplayMode.Detailed,
                DisplayLayout = Enums.DisplayLayout.List
            };
            
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.StoreRooms == null)
                return NotFound();

            var storeRoom = await _context.StoreRooms.FindAsync(id);

            if (storeRoom != null)
            {
                _context.StoreRooms.Remove(storeRoom);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
