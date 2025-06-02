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

        public DisplayViewModel<OwnershipItemViewModel> ViewModel { get; set; } = default!;

        public async Task OnGetAsync()
        {
            ViewData["BasePath"] = "/Ownerships";

            List<OwnershipItemViewModel> ownershipItems = await _context.Ownerships
                .Include(o => o.CompanyTrust)
                .Include(o => o.Unit)
                .ThenInclude(u => u!.Building)
                .AsNoTracking()
                .Select(o => _mapper.Map<OwnershipItemViewModel>(o))
                .ToListAsync();

            ViewModel = new DisplayViewModel<OwnershipItemViewModel>
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