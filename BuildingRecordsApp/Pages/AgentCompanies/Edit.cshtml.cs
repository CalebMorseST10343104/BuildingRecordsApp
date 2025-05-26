using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;
using AutoMapper;

namespace BuildingRecordsApp.Pages.AgentCompanies
{
    public class EditModel : PageModel
    {
        private readonly BuildingContext _context;
        private readonly IMapper _mapper;

        public EditModel(BuildingContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [BindProperty]
        public required AgentCompanyFormViewModel ViewModel { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            var agentCompany = await _context.AgentCompanies
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.AgentCompanyId == id);

            if (agentCompany == null)
                return NotFound();

            ViewModel = _mapper.Map<AgentCompanyFormViewModel>(agentCompany);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ViewModel.AgentCompanyId == null)
                ModelState.AddModelError("ViewModel", "Agent Company details are required.");

            
            if (!ModelState.IsValid)
                return Page();

            // Map the ViewModel to the Entity
            var agentCompany = _mapper.Map<AgentCompany>(ViewModel);
            _context.Attach(agentCompany).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AgentCompanyExists(agentCompany.AgentCompanyId))
                    return NotFound();

                throw;
            }

            return RedirectToPage("/AgentCompanies/Index");
        }

        private bool AgentCompanyExists(int id)
        {
            return _context.AgentCompanies.Any(e => e.AgentCompanyId == id);
        }
    }
}
