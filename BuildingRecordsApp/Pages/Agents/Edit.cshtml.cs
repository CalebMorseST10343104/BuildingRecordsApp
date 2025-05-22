using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BuildingRecordsApp.Models;
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
        public required Agent Agent { get; set; }

        public SelectList? AgentCompanySelectList { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            Agent = await _context.Agents
                .Include(a => a.AgentCompany)
                .FirstOrDefaultAsync(m => m.AgentId == id) ?? null!;

            if (Agent == null)
                return NotFound();

            AgentCompanySelectList = await _selectListService.GetAgentCompanySelectListAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Agent.AgentCompanyId == 0)
                ModelState.AddModelError("Agent.AgentCompanyId", "Please select an agent company.");
            
            if (!ModelState.IsValid)
                return Page();

            _context.Attach(Agent).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AgentExists(Agent!.AgentId))
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
