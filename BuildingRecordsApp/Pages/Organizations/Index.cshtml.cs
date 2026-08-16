using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;
using AutoMapper;
using BuildingRecordsApp.Models.DisplayViewModels;
using BuildingRecordsApp.Models.ItemViewModels;

namespace BuildingRecordsApp.Pages.Organizations
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

        public DisplayViewModel<OrganizationItemViewEntry> ViewModel { get; set; } = default!;

        public async Task OnGetAsync()
        {
            List<OrganizationItemViewEntry> companyTrustItems = await _context.Organizations
                .AsNoTracking()
                .Select(ct => _mapper.Map<OrganizationItemViewEntry>(ct))
                .ToListAsync();

            ViewModel = new DisplayViewModel<OrganizationItemViewEntry>
            {
                Entries = companyTrustItems,
                IdsToDisplay = [.. companyTrustItems.Select(ct => ct.OrganizationId ?? 0)],
                DisplayMode = Enums.DisplayMode.Detailed,
                DisplayLayout = Enums.DisplayLayout.Table,
                ShowActions = true
            };
        }
    }
}