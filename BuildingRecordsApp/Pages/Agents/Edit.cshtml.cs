using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BuildingRecordsApp.Models;
using BuildingRecordsApp.ViewModels;
using Microsoft.EntityFrameworkCore;

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
                    .FirstOrDefaultAsync(m => m.AgentId == id) ?? null!,
                AgentCompanySelectList = new SelectList(Enumerable.Empty<SelectListItem>())
            };

            if (ViewModel == null)
                return NotFound();

            ViewModel.AgentCompanySelectList = await _selectListService.GetAgentCompanySelectListAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ViewModel.Agent.AgentCompanyId == null)
                ModelState.AddModelError("Agent.AgentCompanyId", "Please select an agent company.");
            
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
