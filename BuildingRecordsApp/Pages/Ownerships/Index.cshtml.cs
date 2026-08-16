using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;
using AutoMapper;
using BuildingRecordsApp.Models.DisplayViewModels;
using BuildingRecordsApp.Models.ItemViewModels;

namespace BuildingRecordsApp.Pages.Ownerships
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

        public DisplayViewModel<OwnershipItemViewEntry> ViewModel { get; set; } = default!;

        public async Task OnGetAsync()
        {
            List<OwnershipItemViewEntry> ownershipItems = await _context.Ownerships
                .Include(o => o.Organization)
                .Include(o => o.Unit)
                .ThenInclude(u => u!.Building)
                .ThenInclude(b => b!.Property)
                .AsNoTracking()
                .Select(o => _mapper.Map<OwnershipItemViewEntry>(o))
                .ToListAsync();

            ViewModel = new DisplayViewModel<OwnershipItemViewEntry>
            {
                Entries = ownershipItems,
                IdsToDisplay = [.. ownershipItems.Select(o => o.OwnershipId ?? 0)],
                DisplayMode = Enums.DisplayMode.Detailed,
                DisplayLayout = Enums.DisplayLayout.Table,
                ShowActions = true
            };
        }
    }
}
