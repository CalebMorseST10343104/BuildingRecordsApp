using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BuildingRecordsApp.Pages.Agents
{
    public class EditModel : PageModel
    {
        private readonly BuildingContext _context;
        private readonly ISelectListService _selectListService;
        private readonly IMapper _mapper;

        public EditModel(BuildingContext context, ISelectListService selectListService, IMapper mapper)
        {
            _context = context;
            _selectListService = selectListService;
            _mapper = mapper;
        }

        [BindProperty]
        public required AgentFormViewModel ViewModel { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            var agent = await _context.Agents
                .Include(a => a.AgentCompany)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.AgentId == id);

            if (agent == null)
                return NotFound();

            ViewModel = _mapper.Map<AgentFormViewModel>(agent);
            ViewModel.AgentCompanySelectList = await _selectListService.GetAgentCompanySelectListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ViewModel.AgentId == null)
                ModelState.AddModelError("ViewModel", "Agent details are required.");

            if (ViewModel.AgentCompanyId == null)
                ModelState.AddModelError("ViewModel.AgentCompanyId", "Please select an agent company.");

            if (!ModelState.IsValid)
            {
                ViewModel.AgentCompanySelectList = await _selectListService.GetAgentCompanySelectListAsync();
                return Page();
            }
            
            var agent = _mapper.Map<Agent>(ViewModel);
            _context.Attach(agent).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AgentExists(agent.AgentId))
                    return NotFound();

                throw;
            }

            return RedirectToPage("/Agents/Index");
        }

        private bool AgentExists(int id)
        {
            return _context.Agents.Any(e => e.AgentId == id);
        }
    }
}
