using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models;

namespace BuildingRecordsApp.Pages.Agents
{
    public class CreateModel(BuildingContext context, ISelectListService selectListService) : PageModel
    {
        private readonly BuildingContext _context = context;
        private readonly ISelectListService _selectListService;

        [BindProperty]
        public Agent Agent { get; set; } = new();

        public SelectList AgentCompanySelectList { get; set; } = default!;

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnGetAsync()
        {
            AgentCompanySelectList = await _selectListService.GetAgentCompanySelectListAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            _context.Agents.Add(Agent);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Agents/Index");
        }
    }
}