using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;
using AutoMapper;
using BuildingRecordsApp.Services;
using Microsoft.EntityFrameworkCore;

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

        public async Task<IActionResult> OnGetAsync(int? propertyId)
        {
            ViewModel = new UnitFormViewModel
            {
                PropertyId = propertyId,
                PropertySelectList = await _selectListService.GetPropertySelectListAsync(),
                BuildingSelectList = propertyId.HasValue
                    ? await _selectListService.GetBuildingSelectListAsync(propertyId.Value)
                    : new Microsoft.AspNetCore.Mvc.Rendering.SelectList(Enumerable.Empty<object>())
                ,PersonSelectList = await _selectListService.GetPersonSelectListAsync()
                ,AgentSelectList = await _selectListService.GetAgentSelectListAsync()
            };
            return Page();
        }
        
        public async Task<IActionResult> OnPostAsync()
        {
            if (ViewModel.PropertyId == null)
                ModelState.AddModelError("ViewModel.PropertyId", "Property is required.");
            if (ViewModel.BuildingId == null)
            {
                ModelState.AddModelError("ViewModel.BuildingId", "Building is required.");
            }
            else if (ViewModel.PropertyId is int propertyId && !await _context.Buildings.AnyAsync(
                b => b.BuildingId == ViewModel.BuildingId && b.PropertyId == propertyId))
                ModelState.AddModelError("ViewModel.BuildingId", "Select a building in the chosen property.");

            if (!ModelState.IsValid)
            {
                await ReloadListsAsync();
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
                await ReloadListsAsync();
                return Page();
            }

            return RedirectToPage("/Units/Details", new { id = unit.UnitId });
        }

        public async Task<JsonResult> OnGetBuildingsAsync(int propertyId)
        {
            var buildings = await _context.Buildings.Where(b => b.PropertyId == propertyId)
                .OrderBy(b => b.Name)
                .Select(b => new { value = b.BuildingId, text = b.Name })
                .ToListAsync();
            return new JsonResult(buildings);
        }

        private async Task ReloadListsAsync()
        {
            ViewModel.PropertySelectList = await _selectListService.GetPropertySelectListAsync();
            ViewModel.BuildingSelectList = ViewModel.PropertyId is int propertyId
                ? await _selectListService.GetBuildingSelectListAsync(propertyId)
                : new Microsoft.AspNetCore.Mvc.Rendering.SelectList(Enumerable.Empty<object>());
            ViewModel.PersonSelectList = await _selectListService.GetPersonSelectListAsync();
            ViewModel.AgentSelectList = await _selectListService.GetAgentSelectListAsync();
        }
    }
}
