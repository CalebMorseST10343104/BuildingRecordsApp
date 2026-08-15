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
            if (!ModelState.IsValid)
            {
                ViewModel.PropertySelectList = await _selectListService.GetPropertySelectListAsync();
                return Page();
            }

            var building = _mapper.Map<Building>(ViewModel);

            _context.Buildings.Add(building);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Buildings/Index");
        }
    }
}
