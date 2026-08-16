using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models.Entities;
using AutoMapper;
using BuildingRecordsApp.Models.DisplayViewModels;
using BuildingRecordsApp.Models.ItemViewModels;

namespace BuildingRecordsApp.Pages.AccessDeviceCounts
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

        public DisplayViewModel<AccessDeviceCountItemViewEntry> ViewModel { get; set; } = default!;

        public async Task OnGetAsync()
        {
            List<AccessDeviceCountItemViewEntry> tagRemoteRecordItems = await _context.AccessDeviceCounts
                .Include(t => t.Unit)
                .ThenInclude(u => u!.Building)
                .AsNoTracking()
                .Select(t => _mapper.Map<AccessDeviceCountItemViewEntry>(t))
                .ToListAsync();

            ViewModel = new DisplayViewModel<AccessDeviceCountItemViewEntry>
            {
                Entries = tagRemoteRecordItems,
                IdsToDisplay = [.. tagRemoteRecordItems.Select(t => t.AccessDeviceCountId ?? 0)],
                DisplayMode = Enums.DisplayMode.Detailed,
                DisplayLayout = Enums.DisplayLayout.Table,
                ShowActions = true
            };
        }
    }
}