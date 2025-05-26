using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;
using AutoMapper;

namespace BuildingRecordsApp.Pages.StoreRooms
{
    public class EditModel : PageModel
    {
        private readonly BuildingContext _context;
        private readonly ISelectListService _selectListService;
        private readonly IMapper _mapper;

        public EditModel(BuildingContext context, ISelectListService selectListService, IMapper mapper)
        {
            _context = context;
            _selectListService = selectListService;
            _mapper = mapper;
        }

        [BindProperty]
        public required StoreRoomFormViewModel ViewModel { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            var storeRoom = await _context.StoreRooms
                .Include(sr => sr.Unit)
                .AsNoTracking()
                .FirstOrDefaultAsync(sr => sr.StoreRoomId == id);

            if (storeRoom == null)
                return NotFound();

            ViewModel = _mapper.Map<StoreRoomFormViewModel>(storeRoom);
            ViewModel.UnitSelectList = await _selectListService.GetUnitSelectListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ViewModel.StoreRoomId == null)
                ModelState.AddModelError("ViewModel", "Store Room details are required.");

            if (!ModelState.IsValid)
            {
                ViewModel.UnitSelectList = await _selectListService.GetUnitSelectListAsync();
                return Page();
            }

            var storeRoom = _mapper.Map<StoreRoom>(ViewModel);
            _context.Attach(storeRoom).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StoreRoomExists(storeRoom.StoreRoomId))
                    return NotFound();

                throw;
            }

            return RedirectToPage("/StoreRooms/Index");
        }

        private bool StoreRoomExists(int id)
        {
            return _context.StoreRooms.Any(e => e.StoreRoomId == id);
        }
    }
}
