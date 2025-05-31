using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;
using AutoMapper;

namespace BuildingRecordsApp.Pages.Owners
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

        public List<Owner> Owners { get; set; } = [];

        public async Task OnGetAsync()
        {
            Owners = await _context.Owners
            .Include(o => o.Person)
            .Include(o => o.Ownership)
            .ThenInclude(u => u!.Unit)
            .ThenInclude(u => u!.Building)
            .ToListAsync();
        }
    }
}