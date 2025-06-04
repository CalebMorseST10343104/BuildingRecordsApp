using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using BuildingRecordsApp.Models.DisplayViewModels;
using BuildingRecordsApp.Models.ItemViewModels;
using BuildingRecordsApp.Interfaces;

namespace BuildingRecordsApp.Pages.StoreRooms
{
    public class DeleteModel : PageModel, ISingleDisplay
    {
        private readonly BuildingContext _context;
        private readonly IMapper _mapper;

        public DeleteModel(BuildingContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [BindProperty]
        public DisplayViewModel<StoreRoomItemViewEntry> ViewModel { get; set; } = default!;

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

            ViewModel = new DisplayViewModel<StoreRoomItemViewEntry>
            {
                Entries = [_mapper.Map<StoreRoomItemViewEntry>(storeRoom)],
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
        
        public int GetFirstId()
        {
            if (ViewModel?.Entries != null && ViewModel.Entries.Count > 0)
            {
                return ViewModel.Entries[0].StoreRoomId ?? 0;
            }
            return 0; // Return a default value if no entries are present
        }
    }
}
