using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models.Entities;
using AutoMapper;

namespace BuildingRecordsApp.Pages.TagRemoteRecords
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

        public List<TagRemoteRecord> TagRemoteRecords { get; set; } = new();

        public async Task OnGetAsync()
        {
            TagRemoteRecords = await _context.TagRemoteRecords
            .Include(t => t.Unit)
            .ThenInclude(u => u!.Building)
            .ToListAsync();
        }
    }
}