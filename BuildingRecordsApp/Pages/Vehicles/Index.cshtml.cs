using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models.Entities;
using AutoMapper;

namespace BuildingRecordsApp.Pages.Vehicles
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

        public List<Vehicle> Vehicles { get; set; } = new();

        public async Task OnGetAsync()
        {
            Vehicles = await _context.Vehicles
            .Include(v => v.Unit)
            .ThenInclude(u => u!.Building)
            .ToListAsync();
        }
    }
}