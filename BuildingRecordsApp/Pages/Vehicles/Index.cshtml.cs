using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models.Entities;
using AutoMapper;
using BuildingRecordsApp.Models.DisplayViewModels;
using BuildingRecordsApp.Models.ItemViewModels;

namespace BuildingRecordsApp.Pages.Vehicles
{
    public class IndexModel : PageModel
    {
        private readonly BuildingContext _context;
        private readonly IMapper _mapper;

        public IndexModel(BuildingContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public DisplayViewModel<VehicleItemViewEntry> ViewModel { get; set; } = default!;

        public async Task OnGetAsync()
        {
            List<VehicleItemViewEntry> vehicleItems = await _context.Vehicles
                .Include(v => v.Unit)
                .ThenInclude(u => u!.Building)
                .ThenInclude(b => b!.Property)
                .AsNoTracking()
                .Select(v => _mapper.Map<VehicleItemViewEntry>(v))
                .ToListAsync();

            ViewModel = new DisplayViewModel<VehicleItemViewEntry>
            {
                Entries = vehicleItems,
                IdsToDisplay = [.. vehicleItems.Select(v => v.VehicleId ?? 0)],
                DisplayMode = Enums.DisplayMode.Detailed,
                DisplayLayout = Enums.DisplayLayout.Table,
                ShowActions = true
            };
        }
    }
}
