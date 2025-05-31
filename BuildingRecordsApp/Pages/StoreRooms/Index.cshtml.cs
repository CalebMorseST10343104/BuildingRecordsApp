using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;
using AutoMapper;

namespace BuildingRecordsApp.Pages.StoreRooms
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

        public List<StoreRoom> StoreRooms { get; set; } = new();

        public async Task OnGetAsync()
        {
            StoreRooms = await _context.StoreRooms
            .Include(s => s.Unit)
            .ThenInclude(u => u!.Building)
            .ToListAsync();
        }
    }
}