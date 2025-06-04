using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models.Entities;
using AutoMapper;
using BuildingRecordsApp.Models.DisplayViewModels;
using BuildingRecordsApp.Models.ItemViewModels;

namespace BuildingRecordsApp.Pages.TagRemoteRecords
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

        public DisplayViewModel<TagRemoteRecordItemViewEntry> ViewModel { get; set; } = default!;

        public async Task OnGetAsync()
        {
            ViewData["BasePath"] = "/TagRemoteRecords";

            List<TagRemoteRecordItemViewEntry> tagRemoteRecordItems = await _context.TagRemoteRecords
                .Include(t => t.Unit)
                .ThenInclude(u => u!.Building)
                .AsNoTracking()
                .Select(t => _mapper.Map<TagRemoteRecordItemViewEntry>(t))
                .ToListAsync();

            ViewModel = new DisplayViewModel<TagRemoteRecordItemViewEntry>
            {
                Entries = tagRemoteRecordItems,
                IdsToDisplay = [.. tagRemoteRecordItems.Select(t => t.TagRemoteRecordId ?? 0)],
                DisplayMode = Enums.DisplayMode.Detailed,
                DisplayLayout = Enums.DisplayLayout.Table,
                ShowActions = true
            };
        }
    }
}