using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models.Entities;
using AutoMapper;

namespace BuildingRecordsApp.Pages.Agents
{
    public class IndexModel : PageModel
    {
        private readonly BuildingContext _context;
        private readonly IMapper _mapper;

        public IndexModel(BuildingContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public List<Agent> Agents { get; set; } = new();

        public async Task OnGetAsync()
        {
            Agents = await _context.Agents.ToListAsync();
        }
    }
}