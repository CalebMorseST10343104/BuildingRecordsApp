using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;
using AutoMapper;
using BuildingRecordsApp.Services;

namespace BuildingRecordsApp.Pages.ParkingBays
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
        public required ParkingBayFormViewModel ViewModel { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            var parkingBay = await _context.ParkingBays
                .Include(pb => pb.Unit)
                .AsNoTracking()
                .FirstOrDefaultAsync(pb => pb.ParkingBayId == id);

            if (parkingBay == null)
                return NotFound();

            ViewModel = _mapper.Map<ParkingBayFormViewModel>(parkingBay);
            ViewModel.UnitSelectList = await _selectListService.GetUnitSelectListAsync(ViewModel.PropertyId);
            ViewModel.PropertySelectList = await _selectListService.GetPropertySelectListAsync();
                
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ViewModel.ParkingBayId == null)
                ModelState.AddModelError("ViewModel", "Parking Bay details are required.");

            var bayNumber = ViewModel.ParkingBayNumber?.Trim();
            if (await _context.ParkingBays.AnyAsync(p => p.ParkingBayId != ViewModel.ParkingBayId && p.PropertyId == ViewModel.PropertyId && p.ParkingBayNumber == bayNumber))
                ModelState.AddModelError("ViewModel.ParkingBayNumber", "That parking bay number is already in use in this property.");

            if (!ModelState.IsValid)
            {
                ViewModel.UnitSelectList = await _selectListService.GetUnitSelectListAsync(ViewModel.PropertyId);
            ViewModel.PropertySelectList = await _selectListService.GetPropertySelectListAsync();
                return Page();
            }

            try
            {
                var parkingBay = await _context.ParkingBays.SingleOrDefaultAsync(p => p.ParkingBayId == ViewModel.ParkingBayId.GetValueOrDefault());
                if (parkingBay is null)
                    return NotFound();
                parkingBay.PropertyId = ViewModel.PropertyId;
                parkingBay.ParkingBayNumber = bayNumber ?? string.Empty;
                parkingBay.IsNearEntrance = ViewModel.IsNearEntrance;
                await _allocationService.AllocateParkingBayAsync(parkingBay.ParkingBayId, ViewModel.UnitID);
            }
            catch (BusinessRuleException exception)
            {
                ModelState.AddModelError("ViewModel.UnitID", exception.Message);
                ViewModel.UnitSelectList = await _selectListService.GetUnitSelectListAsync(ViewModel.PropertyId);
            ViewModel.PropertySelectList = await _selectListService.GetPropertySelectListAsync();
                return Page();
            }

            return RedirectToPage("/ParkingBays/Index");
        }

        public async Task<JsonResult> OnGetUnitsAsync(int propertyId) => new(await _context.Units
            .Where(u => u.Building!.PropertyId == propertyId)
            .OrderBy(u => u.Building!.Name).ThenBy(u => u.UnitNumber)
            .Select(u => new { value = u.UnitId, text = $"[{u.Building!.Name}] {u.UnitNumber}" })
            .ToListAsync());
        
        private bool ParkingBayExists(int id)
        {
            return _context.ParkingBays.Any(e => e.ParkingBayId == id);
        }
    }
}
