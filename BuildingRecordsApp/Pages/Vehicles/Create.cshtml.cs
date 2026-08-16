using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace BuildingRecordsApp.Pages.Vehicles
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
        public VehicleFormViewModel ViewModel { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int? unitId)
        {
            ViewModel = new VehicleFormViewModel
            {
                UnitId = unitId,
                UnitSelectList = await _selectListService.GetUnitSelectListAsync()
            };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ViewModel.UnitId == null)
            {
                ModelState.AddModelError("ViewModel.UnitId", "Unit is required.");
            }
            var registration = ViewModel.VehicleRegistration?.Trim();
            if (await _context.Vehicles.AnyAsync(v => v.VehicleRegistration == registration))
                ModelState.AddModelError("ViewModel.VehicleRegistration", "That vehicle registration is already recorded.");
            if (!ModelState.IsValid)
            {
                ViewModel.UnitSelectList = await _selectListService.GetUnitSelectListAsync();
                return Page();
            }

            var vehicle = _mapper.Map<Vehicle>(ViewModel);
            vehicle.VehicleRegistration = registration ?? string.Empty;

            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Units/Details", new { id = ViewModel.UnitId });
        }
    }
}
