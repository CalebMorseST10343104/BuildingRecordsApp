using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;
using AutoMapper;
using BuildingRecordsApp.Models.DisplayViewModels;
using BuildingRecordsApp.Models.ItemViewModels;

namespace BuildingRecordsApp.Pages.Owners
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

        public DisplayViewModel<OwnerItemViewEntry> ViewModel { get; set; } = default!;

        public async Task OnGetAsync()
        {
            List<OwnerItemViewEntry> ownerItems = await _context.Owners
                .Include(o => o.Person)
                .Include(o => o.Ownership)
                .ThenInclude(ow => ow!.Unit)
                .ThenInclude(u => u!.Building)
                .AsNoTracking()
                .Select(o => _mapper.Map<OwnerItemViewEntry>(o))
                .ToListAsync();

            ViewModel = new DisplayViewModel<OwnerItemViewEntry>
            {
                Entries = ownerItems,
                IdsToDisplay = [.. ownerItems.Select(o => o.OwnershipContactId ?? 0)],
                DisplayMode = Enums.DisplayMode.Detailed,
                DisplayLayout = Enums.DisplayLayout.Table,
                ShowActions = true
            };
        }
    }
}