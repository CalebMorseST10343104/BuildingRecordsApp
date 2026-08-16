using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;
using AutoMapper;

namespace BuildingRecordsApp.Pages.Buildings
{
    public class CreateModel : PageModel
    {
        private readonly BuildingContext _context;
        private readonly IMapper _mapper;
        private readonly ISelectListService _selectListService;

        public CreateModel(BuildingContext context, IMapper mapper, ISelectListService selectListService)
        {
            _context = context;
            _mapper = mapper;
            _selectListService = selectListService;
        }

        [BindProperty]
        public BuildingFormViewModel ViewModel { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int? propertyId)
        {
            ViewModel = new BuildingFormViewModel { PropertyId = propertyId.GetValueOrDefault(), PropertySelectList = await _selectListService.GetPropertySelectListAsync() };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!await _context.Properties.AnyAsync(p => p.PropertyId == ViewModel.PropertyId))
                ModelState.AddModelError("ViewModel.PropertyId", "Property is required.");
            var name = ViewModel.Name?.Trim();
            if (await _context.Buildings.AnyAsync(b => b.PropertyId == ViewModel.PropertyId && b.Name == name))
                ModelState.AddModelError("ViewModel.Name", "That building name is already in use in this property.");
            if (!ModelState.IsValid)
            {
                ViewModel.PropertySelectList = await _selectListService.GetPropertySelectListAsync();
                return Page();
            }

            var building = _mapper.Map<Building>(ViewModel);
            building.Name = name ?? string.Empty;

            _context.Buildings.Add(building);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Buildings/Index");
        }
    }
}
