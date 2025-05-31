using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models.Entities;
using AutoMapper;

namespace BuildingRecordsApp.Pages.AgentCompanies
{
    public class DeleteModel : PageModel
    {
        private readonly BuildingContext _context;
        private readonly IMapper _mapper;

        public DeleteModel(BuildingContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [BindProperty]
        public AgentCompany AgentCompany { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.AgentCompanies == null)
            {
                return NotFound();
            }

            var agentcompany = await _context.AgentCompanies
                .Include(a => a.Agents)
                .FirstOrDefaultAsync(m => m.AgentCompanyId == id);

            if (agentcompany == null)
            {
                return NotFound();
            }
            else
            {
                AgentCompany = agentcompany;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.AgentCompanies == null)
            {
                return NotFound();
            }
            var agentcompany = await _context.AgentCompanies.FindAsync(id);

            if (agentcompany != null)
            {
                AgentCompany = agentcompany;
                _context.AgentCompanies.Remove(AgentCompany);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
