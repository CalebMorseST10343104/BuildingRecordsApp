using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;
using AutoMapper;
using BuildingRecordsApp.Services;

namespace BuildingRecordsApp.Pages.Units
{
    public class CreateModel : PageModel
    {
        private readonly BuildingContext _context;
        private readonly ISelectListService _selectListService;
        private readonly IMapper _mapper;
        private readonly IUnitService _unitService;

        public CreateModel(BuildingContext context, ISelectListService selectListService, IMapper mapper, IUnitService unitService)
        {
            _context = context;
            _selectListService = selectListService;
            _mapper = mapper;
            _unitService = unitService;
        }

        [BindProperty]
        public UnitFormViewModel ViewModel { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            ViewModel = new UnitFormViewModel
            {
                BuildingSelectList = await _selectListService.GetBuildingSelectListAsync()
                ,PersonSelectList = await _selectListService.GetPersonSelectListAsync()
                ,AgentSelectList = await _selectListService.GetAgentSelectListAsync()
            };
            return Page();
        }
        
        public async Task<IActionResult> OnPostAsync()
        {
            if (ViewModel.BuildingId == null)
            {
                ModelState.AddModelError("ViewModel.BuildingId", "Building is required.");
            }

            if (!ModelState.IsValid)
            {
                ViewModel.BuildingSelectList = await _selectListService.GetBuildingSelectListAsync();
                ViewModel.PersonSelectList = await _selectListService.GetPersonSelectListAsync();
                ViewModel.AgentSelectList = await _selectListService.GetAgentSelectListAsync();
                return Page();
            }

            var unit = _mapper.Map<Unit>(ViewModel);

            try
            {
                await _unitService.CreateAsync(unit);
            }
            catch (BusinessRuleException exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
                ViewModel.BuildingSelectList = await _selectListService.GetBuildingSelectListAsync();
                ViewModel.PersonSelectList = await _selectListService.GetPersonSelectListAsync();
                ViewModel.AgentSelectList = await _selectListService.GetAgentSelectListAsync();
                return Page();
            }

            return RedirectToPage("/Units/Details", new { id = unit.UnitId });
        }
    }
}
