using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;
using AutoMapper;

namespace BuildingRecordsApp.Pages.Occupancies
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
        public required OccupancyFormViewModel ViewModel { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            var occupancy = await _context.Occupancies
                .Include(o => o.Unit)
                .Include(o => o.Occupant)
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.OccupancyId == id);

            if (occupancy == null)
                return NotFound();

            ViewModel = _mapper.Map<OccupancyFormViewModel>(occupancy);
            ViewModel.UnitSelectList = await _selectListService.GetUnitSelectListAsync(Enums.UsageContext.ForOccupancy);
            ViewModel.PersonSelectList = await _selectListService.GetPersonSelectListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ViewModel.OccupancyId == null)
                ModelState.AddModelError("ViewModel", "Occupancy ID is required.");
            
            if (ViewModel.UnitId == null)
                ModelState.AddModelError("ViewModel.UnitId", "Unit is required.");

            if (ViewModel.UnitId is int unitId && ViewModel.OccupantId is int personId && await _context.Occupancies.AnyAsync(
                o => o.OccupancyId != ViewModel.OccupancyId && o.UnitId == unitId && o.OccupantId == personId))
                ModelState.AddModelError("ViewModel.OccupantId", "This person is already recorded as an occupant of the unit.");

            if (!ModelState.IsValid)
            {
                ViewModel.UnitSelectList = await _selectListService.GetUnitSelectListAsync(Enums.UsageContext.ForOccupancy);
                ViewModel.PersonSelectList = await _selectListService.GetPersonSelectListAsync();
                return Page();
            }

            var occupancy = _mapper.Map<Occupancy>(ViewModel);
            _context.Attach(occupancy).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OccupancyExists(occupancy.OccupancyId))
                    return NotFound();

                throw;
            }

            return RedirectToPage("/Occupancies/Index");
        }

        private bool OccupancyExists(int id)
        {
            return _context.Occupancies.Any(e => e.OccupancyId == id);
        }
    }
}
