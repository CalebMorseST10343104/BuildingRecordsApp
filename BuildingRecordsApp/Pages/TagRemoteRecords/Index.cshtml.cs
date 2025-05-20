using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models;

namespace BuildingRecordsApp.Pages.TagRemoteRecords
{
    public class IndexModel : PageModel
    {
        private readonly BuildingContext _context;

        public IndexModel(BuildingContext context)
        {
            _context = context;
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