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

        public DisplayViewModel<VehicleItemViewModel> ViewModel { get; set; } = default!;

        public async Task OnGetAsync()
        {
            ViewData["BasePath"] = "/Vehicles";

            List<VehicleItemViewModel> vehicleItems = await _context.Vehicles
                .Include(v => v.Unit)
                .ThenInclude(u => u!.Building)
                .AsNoTracking()
                .Select(v => _mapper.Map<VehicleItemViewModel>(v))
                .ToListAsync();

            ViewModel = new DisplayViewModel<VehicleItemViewModel>
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