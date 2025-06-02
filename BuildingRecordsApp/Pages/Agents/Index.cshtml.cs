using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models.Entities;
using AutoMapper;
using BuildingRecordsApp.Models.DisplayViewModels;
using BuildingRecordsApp.Models.ItemViewModels;

namespace BuildingRecordsApp.Pages.Agents
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

        public DisplayViewModel<AgentItemViewModel> ViewModel { get; set; } = default!;

        public async Task OnGetAsync()
        {
            ViewData["BasePath"] = "/Agents";

            List<AgentItemViewModel> agentItems = await _context.Agents
                .AsNoTracking()
                .Select(a => _mapper.Map<AgentItemViewModel>(a))
                .ToListAsync();

            ViewModel = new DisplayViewModel<AgentItemViewModel>
            {
                Entries = agentItems,
                IdsToDisplay = [.. agentItems.Select(a => a.AgentId ?? 0)],
                DisplayMode = Enums.DisplayMode.Detailed,
                DisplayLayout = Enums.DisplayLayout.Table,
                ShowActions = true
            };
        }
    }
}