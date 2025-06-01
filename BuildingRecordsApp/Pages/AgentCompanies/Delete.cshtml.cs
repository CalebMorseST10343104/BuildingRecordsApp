using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models.Entities;
using AutoMapper;
using BuildingRecordsApp.Models.DisplayViewModels;
using BuildingRecordsApp.Models.ItemViewModels;
using BuildingRecordsApp.Interfaces;

namespace BuildingRecordsApp.Pages.AgentCompanies
{
    public class DeleteModel : PageModel, ISingleDisplay
    {
        private readonly BuildingContext _context;
        private readonly IMapper _mapper;

        public DeleteModel(BuildingContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [BindProperty]
        public DisplayViewModel<AgentCompanyItemViewModel> ViewModel { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.AgentCompanies == null)
            {
                return NotFound();
            }

            var agentCompany = await _context.AgentCompanies
                .Include(a => a.Agents)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.AgentCompanyId == id);

            if (agentCompany == null)
                return NotFound();

            ViewModel = new DisplayViewModel<AgentCompanyItemViewModel>
            {
                Entries = [_mapper.Map<AgentCompanyItemViewModel>(agentCompany)],
                IdsToDisplay = [id.Value],
                DisplayMode = Enums.DisplayMode.Detailed,
                DisplayLayout = Enums.DisplayLayout.List
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.AgentCompanies == null)
                return NotFound();

            var agentcompany = await _context.AgentCompanies.FindAsync(id);

            if (agentcompany != null)
            {
                _context.AgentCompanies.Remove(agentcompany);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }

        public int GetFirstId()
        {
            if (ViewModel?.Entries != null && ViewModel.Entries.Count > 0)
            {
                return ViewModel.Entries[0].AgentCompanyId ?? 0;
            }
            return 0; // or throw an exception if no entries are available
        }
    }
}
