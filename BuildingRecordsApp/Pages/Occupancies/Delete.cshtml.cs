using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;
using AutoMapper;
using BuildingRecordsApp.Models.DisplayViewModels;
using BuildingRecordsApp.Models.ItemViewModels;

namespace BuildingRecordsApp.Pages.Occupancies
{
    public class DeleteModel : PageModel
    {
        private readonly BuildingContext _context;
        private readonly IMapper _mapper;

        public DeleteModel(BuildingContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [BindProperty]
        public DisplayViewModel<OccupancyItemViewModel> ViewModel { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            var occupancy = await _context.Occupancies
                .Include(o => o.Unit)
                .ThenInclude(u => u!.Building)
                .Include(o => o.Occupant)
                .FirstOrDefaultAsync(m => m.OccupancyId == id);

            if (occupancy == null)
                return NotFound();
            
            ViewModel = new DisplayViewModel<OccupancyItemViewModel>
            {
                Entries = [_mapper.Map<OccupancyItemViewModel>(occupancy)],
                IdsToDisplay = [occupancy.OccupancyId],
                DisplayMode = Enums.DisplayMode.Detailed,
                DisplayLayout = Enums.DisplayLayout.List
            };
            
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Occupancies == null)
                return NotFound();

            var occupancy = await _context.Occupancies.FindAsync(id);

            if (occupancy != null)
            {
                _context.Occupancies.Remove(occupancy);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
