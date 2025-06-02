using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models.Entities;
using AutoMapper;
using BuildingRecordsApp.Models.DisplayViewModels;
using BuildingRecordsApp.Models.ItemViewModels;

namespace BuildingRecordsApp.Pages.AgentCompanies
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

        public DisplayViewModel<AgentCompanyItemViewModel> ViewModel { get; set; } = default!;

        public async Task OnGetAsync()
        {
            ViewData["BasePath"] = "/AgentCompanies";

            List<AgentCompanyItemViewModel> agentCompanyItems = await _context.AgentCompanies
                .AsNoTracking()
                .Select(ac => _mapper.Map<AgentCompanyItemViewModel>(ac))
                .ToListAsync();

            ViewModel = new DisplayViewModel<AgentCompanyItemViewModel>
            {
                Entries = agentCompanyItems,
                IdsToDisplay = [.. agentCompanyItems.Select(ac => ac.AgentCompanyId ?? 0)],
                DisplayMode = Enums.DisplayMode.Detailed,
                DisplayLayout = Enums.DisplayLayout.Table,
                ShowActions = true
            };
        }
    }
}