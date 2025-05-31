using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models.Entities;
using AutoMapper;

namespace BuildingRecordsApp.Pages.Buildings
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

        public List<Building> Buildings { get; set; } = new();

        public async Task OnGetAsync()
        {
            Buildings = await _context.Buildings.ToListAsync();
        }
    }
}