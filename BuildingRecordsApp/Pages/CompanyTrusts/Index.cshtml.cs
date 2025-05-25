using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;

namespace BuildingRecordsApp.Pages.CompanyTrusts
{
    public class IndexModel : PageModel
    {
        private readonly BuildingContext _context;

        public IndexModel(BuildingContext context)
        {
            _context = context;
        }

        public List<CompanyTrust> CompanyTrusts { get; set; } = new();

        public async Task OnGetAsync()
        {
            CompanyTrusts = await _context.CompanyTrusts.ToListAsync();
        }
    }
}