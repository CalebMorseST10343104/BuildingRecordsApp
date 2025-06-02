using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models.Entities;
using AutoMapper;
using BuildingRecordsApp.Models.DisplayViewModels;
using BuildingRecordsApp.Models.ItemViewModels;

namespace BuildingRecordsApp.Pages.Leases
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

        public DisplayViewModel<LeaseItemViewModel> ViewModel { get; set; } = default!;

        public async Task OnGetAsync()
        {
            ViewData["BasePath"] = "/Leases";

            List<LeaseItemViewModel> leaseItems = await _context.Leases
                .Include(l => l.Unit)
                .ThenInclude(u => u!.Building)
                .AsNoTracking()
                .Select(l => _mapper.Map<LeaseItemViewModel>(l))
                .ToListAsync();

            ViewModel = new DisplayViewModel<LeaseItemViewModel>
            {
                Entries = leaseItems,
                IdsToDisplay = [.. leaseItems.Select(l => l.LeaseId ?? 0)],
                DisplayMode = Enums.DisplayMode.Detailed,
                DisplayLayout = Enums.DisplayLayout.Table,
                ShowActions = true
            };
        }
    }
}