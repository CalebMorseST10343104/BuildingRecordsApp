using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;
using AutoMapper;
using BuildingRecordsApp.Models.DisplayViewModels;
using BuildingRecordsApp.Models.ItemViewModels;

namespace BuildingRecordsApp.Pages.Units
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

        public DisplayViewModel<UnitItemViewEntry> ViewModel { get; set; } = default!;

        public async Task OnGetAsync()
        {
            List<UnitItemViewEntry> unitItems = await _context.Units
                .Include(u => u.Building)
                .AsNoTracking()
                .Select(u => _mapper.Map<UnitItemViewEntry>(u))
                .ToListAsync();

            ViewModel = new DisplayViewModel<UnitItemViewEntry>
            {
                Entries = unitItems,
                IdsToDisplay = [.. unitItems.Select(u => u.UnitId ?? 0)],
                DisplayMode = Enums.DisplayMode.Detailed,
                DisplayLayout = Enums.DisplayLayout.Table,
                ShowActions = true
            };
        }
    }
}
