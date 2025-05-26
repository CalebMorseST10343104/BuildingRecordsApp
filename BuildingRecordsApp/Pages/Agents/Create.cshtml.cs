using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;
using AutoMapper;

namespace BuildingRecordsApp.Pages.Agents
{
    public class CreateModel : PageModel
    {
        private readonly BuildingContext _context;
        private readonly ISelectListService _selectListService;
        private readonly IMapper _mapper;

        public CreateModel(BuildingContext context, ISelectListService selectListService, IMapper mapper)
        {
            _context = context;
            _selectListService = selectListService;
            _mapper = mapper;
        }

        [BindProperty]
        public AgentFormViewModel ViewModel { get; set; } = new AgentFormViewModel();


        public async Task<IActionResult> OnGetAsync()
        {
            ViewModel = new AgentFormViewModel
            {
                AgentCompanySelectList = await _selectListService.GetAgentCompanySelectListAsync()
            };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ViewModel.AgentCompanyId == null)
            {
                ModelState.AddModelError("ViewModel.AgentCompanyId", "Agent Company is required.");
            }

            if (!ModelState.IsValid)
            {
                ViewModel.AgentCompanySelectList = await _selectListService.GetAgentCompanySelectListAsync();
                return Page();
            }

            var agent = _mapper.Map<Agent>(ViewModel);

            _context.Agents.Add(agent);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Agents/Index");
        }
    }
}