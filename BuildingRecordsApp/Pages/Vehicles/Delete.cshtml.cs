using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BuildingRecordsApp.Models.Entities;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using BuildingRecordsApp.Models.DisplayViewModels;
using BuildingRecordsApp.Models.ItemViewModels;

namespace BuildingRecordsApp.Pages.Vehicles
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
        public DisplayViewModel<VehicleItemViewModel> ViewModel { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicles
                .Include(v => v.Unit)
                .ThenInclude(u => u!.Building)
                .FirstOrDefaultAsync(m => m.VehicleId == id);

            if (vehicle == null)
            {
                return NotFound();
            }
            ViewModel = new DisplayViewModel<VehicleItemViewModel>
            {
                Entries = [_mapper.Map<VehicleItemViewModel>(vehicle)],
                IdsToDisplay = [vehicle.VehicleId],
                DisplayMode = Enums.DisplayMode.Detailed,
                DisplayLayout = Enums.DisplayLayout.List
            };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicles.FindAsync(id);

            if (vehicle != null)
            {
                _context.Vehicles.Remove(vehicle);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
