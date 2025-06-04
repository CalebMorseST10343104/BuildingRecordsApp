using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;
using AutoMapper;
using BuildingRecordsApp.Models.DisplayViewModels;
using BuildingRecordsApp.Models.ItemViewModels;

namespace BuildingRecordsApp.Pages.Occupancies
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

        public DisplayViewModel<OccupancyItemViewEntry> ViewModel { get; set; } = default!;

        public async Task OnGetAsync()
        {
            ViewData["BasePath"] = "/Occupancies";

            List<OccupancyItemViewEntry> occupancyItems = await _context.Occupancies
                .Include(o => o.Occupant)
                .Include(o => o.Unit)
                .ThenInclude(u => u!.Building)
                .AsNoTracking()
                .Select(o => _mapper.Map<OccupancyItemViewEntry>(o))
                .ToListAsync();

            ViewModel = new DisplayViewModel<OccupancyItemViewEntry>
            {
                Entries = occupancyItems,
                IdsToDisplay = [.. occupancyItems.Select(o => o.OccupancyId ?? 0)],
                DisplayMode = Enums.DisplayMode.Detailed,
                DisplayLayout = Enums.DisplayLayout.Table,
                ShowActions = true
            };
        }
    }
}