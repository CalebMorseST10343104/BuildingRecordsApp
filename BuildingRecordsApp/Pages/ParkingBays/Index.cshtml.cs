using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;
using AutoMapper;
using BuildingRecordsApp.Models.DisplayViewModels;
using BuildingRecordsApp.Models.ItemViewModels;

namespace BuildingRecordsApp.Pages.ParkingBays
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

        public DisplayViewModel<ParkingBayItemViewModel> ViewModel { get; set; } = default!;

        public async Task OnGetAsync()
        {
            ViewData["BasePath"] = "/ParkingBays";

            List<ParkingBayItemViewModel> parkingBayItems = await _context.ParkingBays
                .Include(p => p.Unit)
                .ThenInclude(u => u!.Building)
                .AsNoTracking()
                .Select(p => _mapper.Map<ParkingBayItemViewModel>(p))
                .ToListAsync();

            ViewModel = new DisplayViewModel<ParkingBayItemViewModel>
            {
                Entries = parkingBayItems,
                IdsToDisplay = [.. parkingBayItems.Select(p => p.ParkingBayId ?? 0)],
                DisplayMode = Enums.DisplayMode.Detailed,
                DisplayLayout = Enums.DisplayLayout.Table,
                ShowActions = true
            };
        }
    }
}