using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models;

namespace BuildingRecordsApp.Pages.Agents
{
    public class CreateModel : PageModel
    {
        private readonly BuildingContext _context;
        private readonly ISelectListService _selectListService;

        public CreateModel(BuildingContext context, ISelectListService selectListService)
        {
            _context = context;
            _selectListService = selectListService;
        }

        [BindProperty]
        public Agent Agent { get; set; } = new();

        public SelectList AgentCompanySelectList { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            AgentCompanySelectList = await _selectListService.GetAgentCompanySelectListAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Agent.AgentCompanyId == 0)
                ModelState.AddModelError("Agent.AgentCompanyId", "Please select an agent company.");
                
            if (!ModelState.IsValid)
                return Page();

            _context.Agents.Add(Agent);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Agents/Index");
        }
    }
}