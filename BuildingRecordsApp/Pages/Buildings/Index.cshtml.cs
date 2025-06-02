using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models.Entities;
using AutoMapper;
using BuildingRecordsApp.Models.DisplayViewModels;
using BuildingRecordsApp.Models.ItemViewModels;

namespace BuildingRecordsApp.Pages.Buildings
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

        public DisplayViewModel<BuildingItemViewModel> ViewModel { get; set; } = default!;

        public async Task OnGetAsync()
        {
            ViewData["BasePath"] = "/Buildings";

            List<BuildingItemViewModel> buildingItems = await _context.Buildings
                .AsNoTracking()
                .Select(b => _mapper.Map<BuildingItemViewModel>(b))
                .ToListAsync();

            ViewModel = new DisplayViewModel<BuildingItemViewModel>
            {
                Entries = buildingItems,
                IdsToDisplay = [.. buildingItems.Select(b => b.BuildingId ?? 0)],
                DisplayMode = Enums.DisplayMode.Detailed,
                DisplayLayout = Enums.DisplayLayout.Table,
                ShowActions = true
            };
        }
    }
}