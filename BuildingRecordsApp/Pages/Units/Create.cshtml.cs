using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;
using AutoMapper;

namespace BuildingRecordsApp.Pages.Units
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
        public UnitFormViewModel ViewModel { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            ViewModel = new UnitFormViewModel
            {
                BuildingSelectList = await _selectListService.GetBuildingSelectListAsync()
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
                return Page();
            }

            var unit = _mapper.Map<Unit>(ViewModel);

            _context.Units.Add(unit);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Units/Index");
        }
    }
}
