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

        public DisplayViewModel<TagRemoteRecordItemViewModel> ViewModel { get; set; } = default!;

        public async Task OnGetAsync()
        {
            ViewData["BasePath"] = "/TagRemoteRecords";

            List<TagRemoteRecordItemViewModel> tagRemoteRecordItems = await _context.TagRemoteRecords
                .Include(t => t.Unit)
                .ThenInclude(u => u!.Building)
                .AsNoTracking()
                .Select(t => _mapper.Map<TagRemoteRecordItemViewModel>(t))
                .ToListAsync();

            ViewModel = new DisplayViewModel<TagRemoteRecordItemViewModel>
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