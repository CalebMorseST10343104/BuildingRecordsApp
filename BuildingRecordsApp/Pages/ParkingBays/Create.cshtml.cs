using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;
using AutoMapper;

namespace BuildingRecordsApp.Pages.ParkingBays
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
        public ParkingBayFormViewModel ViewModel { get; set; } = new();
        [BindProperty]
        public bool ReturnToProperty { get; set; }

        public async Task<IActionResult> OnGetAsync(int? propertyId)
        {
            ReturnToProperty = propertyId.HasValue;
            ViewModel = new ParkingBayFormViewModel
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
            var bayNumber = ViewModel.ParkingBayNumber?.Trim();
            if (await _context.ParkingBays.AnyAsync(p => p.PropertyId == ViewModel.PropertyId && p.ParkingBayNumber == bayNumber))
                ModelState.AddModelError("ViewModel.ParkingBayNumber", "That parking bay number is already in use in this property.");
            if (ViewModel.UnitID is int unitId)
            {
                var unitPropertyId = await _context.Units.Where(u => u.UnitId == unitId).Select(u => u.Building!.PropertyId).SingleOrDefaultAsync();
                if (unitPropertyId != ViewModel.PropertyId)
                    ModelState.AddModelError("ViewModel.UnitID", "The unit must be in the same property as the parking bay.");
            }

            if (!ModelState.IsValid)
            {
                ViewModel.UnitSelectList = await _selectListService.GetUnitSelectListAsync(ViewModel.PropertyId);
                ViewModel.PropertySelectList = await _selectListService.GetPropertySelectListAsync();
                return Page();
            }

            var parkingBay = _mapper.Map<ParkingBay>(ViewModel);
            parkingBay.ParkingBayNumber = bayNumber ?? string.Empty;

            _context.ParkingBays.Add(parkingBay);
            await _context.SaveChangesAsync();

            return ReturnToProperty
                ? RedirectToPage("/Properties/Details", new { id = ViewModel.PropertyId })
                : RedirectToPage("/ParkingBays/Index");
        }

        public async Task<JsonResult> OnGetUnitsAsync(int propertyId) => new(await _context.Units
            .Where(u => u.Building!.PropertyId == propertyId)
            .OrderBy(u => u.Building!.Name).ThenBy(u => u.UnitNumber)
            .Select(u => new { value = u.UnitId, text = $"[{u.Building!.Name}] {u.UnitNumber}" })
            .ToListAsync());
    }
}
