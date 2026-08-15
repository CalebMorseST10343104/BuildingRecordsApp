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

        public CreateModel(BuildingContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [BindProperty]
        public BuildingFormViewModel ViewModel { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            ViewModel = new BuildingFormViewModel { PropertyId = await _context.Properties.Select(p => p.PropertyId).FirstAsync() };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!await _context.Properties.AnyAsync(p => p.PropertyId == ViewModel.PropertyId))
                ModelState.AddModelError("ViewModel.PropertyId", "Property is required.");
            if (!ModelState.IsValid)
                return Page();

            var building = _mapper.Map<Building>(ViewModel);

            _context.Buildings.Add(building);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Buildings/Index");
        }
    }
}
