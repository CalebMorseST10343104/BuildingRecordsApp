using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;
using AutoMapper;
using BuildingRecordsApp.Services;

namespace BuildingRecordsApp.Pages.Agents
{
    public class CreateModel : PageModel
    {
        private readonly BuildingContext _context;
        private readonly ISelectListService _selectListService;
        private readonly IMapper _mapper;
        private readonly IAgentService _agentService;

        public CreateModel(BuildingContext context, ISelectListService selectListService, IMapper mapper, IAgentService agentService)
        {
            _context = context;
            _selectListService = selectListService;
            _mapper = mapper;
            _agentService = agentService;
        }

        [BindProperty]
        public AgentFormViewModel ViewModel { get; set; } = new AgentFormViewModel();


        public async Task<IActionResult> OnGetAsync()
        {
            ViewModel = new AgentFormViewModel
            {
                AgentCompanySelectList = await _selectListService.GetAgentCompanySelectListAsync(),
                PersonSelectList = await _selectListService.GetPersonSelectListAsync()
            };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ViewModel.AgentCompanyId == null)
            {
                ModelState.AddModelError("ViewModel.AgentCompanyId", "Agent Company is required.");
            }
            if (ViewModel.PersonId == null)
                ModelState.AddModelError("ViewModel.PersonId", "Person is required.");

            if (!ModelState.IsValid)
            {
                ViewModel.AgentCompanySelectList = await _selectListService.GetAgentCompanySelectListAsync();
                ViewModel.PersonSelectList = await _selectListService.GetPersonSelectListAsync();
                return Page();
            }

            try
            {
                await _agentService.CreateProfileAsync(ViewModel.PersonId!.Value, ViewModel.AgentCompanyId!.Value);
            }
            catch (BusinessRuleException exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
                ViewModel.AgentCompanySelectList = await _selectListService.GetAgentCompanySelectListAsync();
                ViewModel.PersonSelectList = await _selectListService.GetPersonSelectListAsync();
                return Page();
            }

            return RedirectToPage("/Agents/Index");
        }
    }
}
