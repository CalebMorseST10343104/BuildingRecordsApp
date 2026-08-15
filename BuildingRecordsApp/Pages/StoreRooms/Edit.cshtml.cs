using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;
using AutoMapper;
using BuildingRecordsApp.Services;

namespace BuildingRecordsApp.Pages.StoreRooms
{
    public class EditModel : PageModel
    {
        private readonly BuildingContext _context;
        private readonly ISelectListService _selectListService;
        private readonly IMapper _mapper;
        private readonly IPropertyAllocationService _allocationService;

        public EditModel(BuildingContext context, ISelectListService selectListService, IMapper mapper, IPropertyAllocationService allocationService)
        {
            _context = context;
            _selectListService = selectListService;
            _mapper = mapper;
            _allocationService = allocationService;
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

            try
            {
                var storeRoom = await _context.StoreRooms.SingleOrDefaultAsync(s => s.StoreRoomId == ViewModel.StoreRoomId.GetValueOrDefault());
                if (storeRoom is null)
                    return NotFound();
                storeRoom.StoreRoomNumber = ViewModel.StoreRoomNumber?.Trim() ?? string.Empty;
                await _allocationService.AllocateStoreRoomAsync(storeRoom.StoreRoomId, ViewModel.UnitId);
            }
            catch (BusinessRuleException exception)
            {
                ModelState.AddModelError("ViewModel.UnitId", exception.Message);
                ViewModel.UnitSelectList = await _selectListService.GetUnitSelectListAsync();
                return Page();
            }

            return RedirectToPage("/StoreRooms/Index");
        }

        private bool StoreRoomExists(int id)
        {
            return _context.StoreRooms.Any(e => e.StoreRoomId == id);
        }
    }
}
