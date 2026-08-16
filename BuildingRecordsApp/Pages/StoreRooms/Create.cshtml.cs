using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace BuildingRecordsApp.Pages.StoreRooms
{
    public class CreateModel : PageModel
    {
        private readonly BuildingContext _context;
        private readonly ISelectListService _selectListService;
        private readonly IMapper _mapper;

        public CreateModel(BuildingContext context, ISelectListService selectListService, IMapper mapper)
        {
            _context = context;
            _selectListService = selectListService;
            _mapper = mapper;
        }

        [BindProperty]
        public StoreRoomFormViewModel ViewModel { get; set; } = new();
        [BindProperty]
        public bool ReturnToProperty { get; set; }

        public async Task<IActionResult> OnGetAsync(int? propertyId)
        {
            ReturnToProperty = propertyId.HasValue;
            ViewModel = new StoreRoomFormViewModel
            {
                PropertyId = propertyId.GetValueOrDefault(),
                PropertySelectList = await _selectListService.GetPropertySelectListAsync(),
                UnitSelectList = propertyId.HasValue
                    ? await _selectListService.GetUnitSelectListAsync(propertyId.Value)
                    : new SelectList(Enumerable.Empty<object>())
            };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!await _context.Properties.AnyAsync(p => p.PropertyId == ViewModel.PropertyId))
                ModelState.AddModelError("ViewModel.PropertyId", "Property is required.");
            var roomNumber = ViewModel.StoreRoomNumber?.Trim();
            if (await _context.StoreRooms.AnyAsync(s => s.PropertyId == ViewModel.PropertyId && s.StoreRoomNumber == roomNumber))
                ModelState.AddModelError("ViewModel.StoreRoomNumber", "That storeroom number is already in use in this property.");
            if (ViewModel.UnitId is int unitId)
            {
                var unitPropertyId = await _context.Units.Where(u => u.UnitId == unitId).Select(u => u.Building!.PropertyId).SingleOrDefaultAsync();
                if (unitPropertyId != ViewModel.PropertyId)
                    ModelState.AddModelError("ViewModel.UnitId", "The unit must be in the same property as the storeroom.");
            }
            if (!ModelState.IsValid)
            {
                ViewModel.UnitSelectList = await _selectListService.GetUnitSelectListAsync(ViewModel.PropertyId);
                ViewModel.PropertySelectList = await _selectListService.GetPropertySelectListAsync();
                return Page();
            }

            var storeRoom = _mapper.Map<StoreRoom>(ViewModel);
            storeRoom.StoreRoomNumber = roomNumber ?? string.Empty;

            _context.StoreRooms.Add(storeRoom);
            await _context.SaveChangesAsync();

            return ReturnToProperty
                ? RedirectToPage("/Properties/Details", new { id = ViewModel.PropertyId })
                : RedirectToPage("/StoreRooms/Index");
        }

        public async Task<JsonResult> OnGetUnitsAsync(int propertyId) => new(await _context.Units
            .Where(u => u.Building!.PropertyId == propertyId)
            .OrderBy(u => u.Building!.Name).ThenBy(u => u.UnitNumber)
            .Select(u => new { value = u.UnitId, text = $"[{u.Building!.Name}] {u.UnitNumber}" })
            .ToListAsync());
    }
}
