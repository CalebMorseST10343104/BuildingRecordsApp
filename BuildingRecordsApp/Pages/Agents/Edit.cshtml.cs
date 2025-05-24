using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models;
using BuildingRecordsApp.ViewModels;

namespace BuildingRecordsApp.Pages.Agents
{
    public class EditModel : PageModel
    {
        private readonly BuildingContext _context;
        private readonly ISelectListService _selectListService;

        public EditModel(BuildingContext context, ISelectListService selectListService)
        {
            _context = context;
            _selectListService = selectListService;
        }

        [BindProperty]
        public required AgentFormViewModel ViewModel { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            ViewModel = new AgentFormViewModel
            {
                Agent = await _context.Agents
                    .Include(a => a.AgentCompany)
                    .FirstOrDefaultAsync(m => m.AgentId == id),
                AgentCompanySelectList = await _selectListService.GetAgentCompanySelectListAsync()
            };

            if (ViewModel.Agent == null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ViewModel == null)
            {
                ModelState.AddModelError("ViewModel", "Agent details are required.");
                return Page();
            }
            if (ViewModel.Agent == null)
            {
                ModelState.AddModelError("ViewModel.Agent", "Agent details are required.");
                return Page();
            }
            if (ViewModel.Agent.AgentCompanyId == null)
            {
                ModelState.AddModelError("ViewModelAgent.AgentCompanyId", "Please select an agent company.");
                return Page();
            }
            
            if (!ModelState.IsValid)
                return Page();

            _context.Attach(ViewModel.Agent).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AgentExists(ViewModel.Agent.AgentId))
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
