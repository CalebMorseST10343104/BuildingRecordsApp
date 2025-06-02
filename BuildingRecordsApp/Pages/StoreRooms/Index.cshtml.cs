using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;
using AutoMapper;
using BuildingRecordsApp.Models.DisplayViewModels;
using BuildingRecordsApp.Models.ItemViewModels;

namespace BuildingRecordsApp.Pages.StoreRooms
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

        public DisplayViewModel<StoreRoomItemViewModel> ViewModel { get; set; } = default!;

        public async Task OnGetAsync()
        {
            ViewData["BasePath"] = "/StoreRooms";

            List<StoreRoomItemViewModel> storeRoomItems = await _context.StoreRooms
                .Include(s => s.Unit)
                .ThenInclude(u => u!.Building)
                .AsNoTracking()
                .Select(s => _mapper.Map<StoreRoomItemViewModel>(s))
                .ToListAsync();

            ViewModel = new DisplayViewModel<StoreRoomItemViewModel>
            {
                Entries = storeRoomItems,
                IdsToDisplay = [.. storeRoomItems.Select(s => s.StoreRoomId ?? 0)],
                DisplayMode = Enums.DisplayMode.Detailed,
                DisplayLayout = Enums.DisplayLayout.Table,
                ShowActions = true
            };
        }
    }
}