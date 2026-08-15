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
            ViewModel.UnitSelectList = await _selectListService.GetUnitSelectListAsync();
                
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ViewModel.ParkingBayId == null)
                ModelState.AddModelError("ViewModel", "Parking Bay details are required.");

            if (!ModelState.IsValid)
            {
                ViewModel.UnitSelectList = await _selectListService.GetUnitSelectListAsync();
                return Page();
            }

            try
            {
                var parkingBay = await _context.ParkingBays.SingleOrDefaultAsync(p => p.ParkingBayId == ViewModel.ParkingBayId.GetValueOrDefault());
                if (parkingBay is null)
                    return NotFound();
                parkingBay.ParkingBayNumber = ViewModel.ParkingBayNumber?.Trim() ?? string.Empty;
                parkingBay.IsNearEntrance = ViewModel.IsNearEntrance;
                await _allocationService.AllocateParkingBayAsync(parkingBay.ParkingBayId, ViewModel.UnitID);
            }
            catch (BusinessRuleException exception)
            {
                ModelState.AddModelError("ViewModel.UnitID", exception.Message);
                ViewModel.UnitSelectList = await _selectListService.GetUnitSelectListAsync();
                return Page();
            }

            return RedirectToPage("/ParkingBays/Index");
        }
        
        private bool ParkingBayExists(int id)
        {
            return _context.ParkingBays.Any(e => e.ParkingBayId == id);
        }
    }
}
