using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;
using AutoMapper;
using BuildingRecordsApp.Models.DisplayViewModels;
using BuildingRecordsApp.Models.ItemViewModels;

namespace BuildingRecordsApp.Pages.CompanyTrusts
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

        public DisplayViewModel<CompanyTrustItemViewEntry> ViewModel { get; set; } = default!;

        public async Task OnGetAsync()
        {
            ViewData["BasePath"] = "/CompanyTrusts";

            List<CompanyTrustItemViewEntry> companyTrustItems = await _context.CompanyTrusts
                .AsNoTracking()
                .Select(ct => _mapper.Map<CompanyTrustItemViewEntry>(ct))
                .ToListAsync();

            ViewModel = new DisplayViewModel<CompanyTrustItemViewEntry>
            {
                Entries = companyTrustItems,
                IdsToDisplay = [.. companyTrustItems.Select(ct => ct.CompanyTrustId ?? 0)],
                DisplayMode = Enums.DisplayMode.Detailed,
                DisplayLayout = Enums.DisplayLayout.Table,
                ShowActions = true
            };
        }
    }
}