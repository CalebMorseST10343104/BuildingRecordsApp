using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BuildingRecordsApp.Models;
using BuildingRecordsApp.Models.FormViewModels;
using AutoMapper;
using BuildingRecordsApp.Models.Entities;

namespace BuildingRecordsApp.Pages.AgentCompanies
{
    public class CreateModel : PageModel
    {
        private readonly BuildingContext _context;
        private readonly IMapper _mapper;

        public CreateModel(BuildingContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [BindProperty]
        public AgentCompanyFormViewModel ViewModel { get; set; } = new();

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            // Map the ViewModel to the Entity
            var agentCompany = _mapper.Map<AgentCompany>(ViewModel);

            _context.AgentCompanies.Add(agentCompany);
            await _context.SaveChangesAsync();

            return RedirectToPage("/AgentCompanies/Index");
        }
    }
}