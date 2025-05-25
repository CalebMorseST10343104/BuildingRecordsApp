using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;

namespace BuildingRecordsApp.Pages.Persons
{
    public class IndexModel : PageModel
    {
        private readonly BuildingContext _context;

        public IndexModel(BuildingContext context)
        {
            _context = context;
        }

        public List<Person> Persons { get; set; } = [];

        public async Task OnGetAsync()
        {
            Persons = await _context.Persons.ToListAsync();
        }
    }
}