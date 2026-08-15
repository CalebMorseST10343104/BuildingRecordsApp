using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;
using AutoMapper;

namespace BuildingRecordsApp.Pages.Occupancies
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
        public OccupancyFormViewModel ViewModel { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int? unitId)
        {
            ViewModel = new OccupancyFormViewModel
            {
                UnitId = unitId,
                UnitSelectList = await _selectListService.GetUnitSelectListAsync(Enums.UsageContext.ForOccupancy),
                PersonSelectList = await _selectListService.GetPersonSelectListAsync()
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ViewModel.UnitId == null)
            {
                ModelState.AddModelError("ViewModel.UnitId", "Unit is required.");
            }

            if (!ModelState.IsValid)
            {
                ViewModel.UnitSelectList = await _selectListService.GetUnitSelectListAsync(Enums.UsageContext.ForOccupancy);
                ViewModel.PersonSelectList = await _selectListService.GetPersonSelectListAsync();
                return Page();
            }

            var occupancy = _mapper.Map<Occupancy>(ViewModel);

            _context.Occupancies.Add(occupancy);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Units/Details", new { id = ViewModel.UnitId });
        }
    }
}
