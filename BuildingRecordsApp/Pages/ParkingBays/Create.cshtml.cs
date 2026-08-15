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

        public async Task<IActionResult> OnGetAsync(int? propertyId)
        {
            ViewModel = new ParkingBayFormViewModel
            {
                PropertyId = propertyId.GetValueOrDefault(),
                PropertySelectList = await _selectListService.GetPropertySelectListAsync(),
                UnitSelectList = await _selectListService.GetUnitSelectListAsync()
            };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!await _context.Properties.AnyAsync(p => p.PropertyId == ViewModel.PropertyId))
                ModelState.AddModelError("ViewModel.PropertyId", "Property is required.");
            if (ViewModel.UnitID is int unitId)
            {
                var unitPropertyId = await _context.Units.Where(u => u.UnitId == unitId).Select(u => u.Building!.PropertyId).SingleOrDefaultAsync();
                if (unitPropertyId != ViewModel.PropertyId)
                    ModelState.AddModelError("ViewModel.UnitID", "The unit must be in the same property as the parking bay.");
            }

            if (!ModelState.IsValid)
            {
                ViewModel.UnitSelectList = await _selectListService.GetUnitSelectListAsync();
                ViewModel.PropertySelectList = await _selectListService.GetPropertySelectListAsync();
                return Page();
            }

            var parkingBay = _mapper.Map<ParkingBay>(ViewModel);

            _context.ParkingBays.Add(parkingBay);
            await _context.SaveChangesAsync();

            return RedirectToPage("/ParkingBays/Index");
        }
    }
}
