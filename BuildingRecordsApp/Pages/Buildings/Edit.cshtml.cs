using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;
using AutoMapper;

namespace BuildingRecordsApp.Pages.Buildings
{
    public class EditModel : PageModel
    {
        private readonly BuildingContext _context;
        private readonly IMapper _mapper;
        private readonly ISelectListService _selectListService;

        public EditModel(BuildingContext context, IMapper mapper, ISelectListService selectListService)
        {
            _context = context;
            _mapper = mapper;
            _selectListService = selectListService;
        }

        [BindProperty]
        public required BuildingFormViewModel ViewModel { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            var building = await _context.Buildings
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.BuildingId == id);

            if (building == null)
                return NotFound();

            ViewModel = _mapper.Map<BuildingFormViewModel>(building);
            ViewModel.PropertySelectList = await _selectListService.GetPropertySelectListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ViewModel.BuildingId == null)
                ModelState.AddModelError("ViewModel", "Building details are required.");

            if (!await _context.Properties.AnyAsync(p => p.PropertyId == ViewModel.PropertyId))
                ModelState.AddModelError("ViewModel.PropertyId", "Property is required.");

            var name = ViewModel.Name?.Trim();
            if (await _context.Buildings.AnyAsync(b => b.BuildingId != ViewModel.BuildingId && b.PropertyId == ViewModel.PropertyId && b.Name == name))
                ModelState.AddModelError("ViewModel.Name", "That building name is already in use in this property.");

            if (ViewModel.BuildingId is int buildingId && await _context.Units
                .Where(u => u.BuildingId == buildingId)
                .AnyAsync(u => u.ParkingBays.Any(p => p.PropertyId != ViewModel.PropertyId)
                    || u.StoreRooms.Any(s => s.PropertyId != ViewModel.PropertyId)))
            {
                ModelState.AddModelError("ViewModel.PropertyId", "This building has units with parking bays or storerooms in its current property. Reassign those records before moving the building.");
            }
            
            if (!ModelState.IsValid)
            {
                ViewModel.PropertySelectList = await _selectListService.GetPropertySelectListAsync();
                return Page();
            }

            var building = _mapper.Map<Building>(ViewModel);
            building.Name = name ?? string.Empty;
            _context.Attach(building).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BuildingExists(building.BuildingId))
                    return NotFound();

                throw;
            }

            return RedirectToPage("/Buildings/Index");
        }
        
        private bool BuildingExists(int id)
        {
            return _context.Buildings.Any(e => e.BuildingId == id);
        }
    }
}
