using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models;
using BuildingRecordsApp.ViewModels;

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
        public AgentFormViewModel ViewModel { get; set; } = new AgentFormViewModel();


        public async Task<IActionResult> OnGetAsync()
        {
            ViewModel = new AgentFormViewModel
            {
                Agent = new Agent(),
                AgentCompanySelectList = await _selectListService.GetAgentCompanySelectListAsync()
            };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ViewModel.Agent.AgentCompanyId == null)
                ModelState.AddModelError("Agent.AgentCompanyId", "Please select an agent company.");
                
            if (!ModelState.IsValid)
                return Page();

            _context.Agents.Add(ViewModel.Agent);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Agents/Index");
        }
    }
}