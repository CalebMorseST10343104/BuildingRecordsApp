using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;
using AutoMapper;

namespace BuildingRecordsApp.Pages.ParkingBays
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

        public List<ParkingBay> ParkingBays { get; set; } = new();

        public async Task OnGetAsync()
        {
            ParkingBays = await _context.ParkingBays
            .Include(p => p.Unit)
            .ThenInclude(u => u!.Building)
            .ToListAsync();
        }
    }
}