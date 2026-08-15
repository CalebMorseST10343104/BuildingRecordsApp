using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;
using AutoMapper;

namespace BuildingRecordsApp.Pages.Units
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
        public UnitFormViewModel ViewModel { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            var unit = await _context.Units
                .Include(u => u.Building)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.UnitId == id);

            if (unit == null)
                return NotFound();

            ViewModel = _mapper.Map<UnitFormViewModel>(unit);
            ViewModel.BuildingSelectList = await _selectListService.GetBuildingSelectListAsync();
            ViewModel.PersonSelectList = await _selectListService.GetPersonSelectListAsync();
            ViewModel.AgentSelectList = await _selectListService.GetAgentSelectListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ViewModel.UnitId == null)
                ModelState.AddModelError("ViewModel", "Unit details are required.");
                
            if (ViewModel.BuildingId == null)
                ModelState.AddModelError("ViewModel.BuildingId", "Building is required.");

            if (!ModelState.IsValid)
            {
                ViewModel.BuildingSelectList = await _selectListService.GetBuildingSelectListAsync();
                ViewModel.PersonSelectList = await _selectListService.GetPersonSelectListAsync();
                ViewModel.AgentSelectList = await _selectListService.GetAgentSelectListAsync();
                return Page();
            }

            var unit = _mapper.Map<Unit>(ViewModel);
            _context.Attach(unit).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UnitExists(unit.UnitId))
                    return NotFound();

                throw;
            }
            return RedirectToPage("/Units/Details", new { id = unit.UnitId });
        }

        private bool UnitExists(int id)
        {
            return _context.Units.Any(e => e.UnitId == id);
        }
    }
}
