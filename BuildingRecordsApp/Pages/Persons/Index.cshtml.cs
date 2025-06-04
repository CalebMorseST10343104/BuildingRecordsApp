using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;
using AutoMapper;
using BuildingRecordsApp.Models.DisplayViewModels;
using BuildingRecordsApp.Models.ItemViewModels;

namespace BuildingRecordsApp.Pages.Persons
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

        public DisplayViewModel<PersonItemViewEntry> ViewModel { get; set; } = default!;

        public async Task OnGetAsync()
        {
            List<PersonItemViewEntry> personItems = await _context.Persons
                .AsNoTracking()
                .Select(p => _mapper.Map<PersonItemViewEntry>(p))
                .ToListAsync();

            ViewModel = new DisplayViewModel<PersonItemViewEntry>
            {
                Entries = personItems,
                IdsToDisplay = [.. personItems.Select(p => p.PersonId ?? 0)],
                DisplayMode = Enums.DisplayMode.Detailed,
                DisplayLayout = Enums.DisplayLayout.Table,
                ShowActions = true
            };
        }
    }
}