using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;
using AutoMapper;

namespace BuildingRecordsApp.Pages.Vehicles
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
        public required VehicleFormViewModel ViewModel { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            var vehicle = await _context.Vehicles
                .Include(v => v.Unit)
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.VehicleId == id);

            if (vehicle == null)
                return NotFound();

            ViewModel = _mapper.Map<VehicleFormViewModel>(vehicle);
            ViewModel.UnitSelectList = await _selectListService.GetUnitSelectListAsync();
                
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ViewModel.VehicleId == null)
                ModelState.AddModelError("Vehicle", "Vehicle details are required.");
                
            if (ViewModel.UnitId == null)
                ModelState.AddModelError("Vehicle.UnitId", "Unit is required.");

            if (!ModelState.IsValid)
            {
                ViewModel.UnitSelectList = await _selectListService.GetUnitSelectListAsync();
                return Page();
            }

            var vehicle = _mapper.Map<Vehicle>(ViewModel);
            _context.Attach(vehicle).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VehicleExists(vehicle.VehicleId))
                    return NotFound();

                throw;
            }

            return RedirectToPage("/Vehicles/Index");
        }

        private bool VehicleExists(int id)
        {
            return _context.Vehicles.Any(e => e.VehicleId == id);
        }
    }
}
