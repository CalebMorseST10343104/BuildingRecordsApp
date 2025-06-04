using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;
using AutoMapper;
using BuildingRecordsApp.Models.DisplayViewModels;
using BuildingRecordsApp.Models.ItemViewModels;
using BuildingRecordsApp.Interfaces;

namespace BuildingRecordsApp.Pages.Persons
{
    public class DeleteModel : PageModel, ISingleDisplay
    {
        private readonly BuildingContext _context;
        private readonly IMapper _mapper;

        public DeleteModel(BuildingContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [BindProperty]
        public DisplayViewModel<PersonItemViewEntry> ViewModel { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            var Person = await _context.Persons
                .FirstOrDefaultAsync(m => m.PersonId == id);

            if (Person == null)
                return NotFound();

            ViewModel = new DisplayViewModel<PersonItemViewEntry>
            {
                Entries = [_mapper.Map<PersonItemViewEntry>(Person)],
                IdsToDisplay = [Person.PersonId],
                DisplayMode = Enums.DisplayMode.Detailed,
                DisplayLayout = Enums.DisplayLayout.List
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Persons == null)
                return NotFound();

            var person = await _context.Persons.FindAsync(id);

            if (person != null)
            {
                _context.Persons.Remove(person);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }

        public int GetFirstId()
        {
            if (ViewModel?.Entries != null && ViewModel.Entries.Count > 0)
            {
                return ViewModel.Entries[0].PersonId ?? 0;
            }
            return 0;
        }
    }
}
