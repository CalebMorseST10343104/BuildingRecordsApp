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

        public EditModel(BuildingContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ViewModel.BuildingId == null)
                ModelState.AddModelError("ViewModel", "Building details are required.");
            
            if (!ModelState.IsValid)
                return Page();

            var building = _mapper.Map<Building>(ViewModel);
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
