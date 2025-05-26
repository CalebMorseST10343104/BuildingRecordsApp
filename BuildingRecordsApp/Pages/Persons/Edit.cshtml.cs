using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;
using AutoMapper;

namespace BuildingRecordsApp.Pages.Persons
{
    public class EditModel : PageModel
    {
        private readonly BuildingContext _context;
        private readonly IMapper _mapper;

        public EditModel(BuildingContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [BindProperty]
        public required PersonFormViewModel ViewModel { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            var person = await _context.Persons
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.PersonId == id);

            if (person == null)
                return NotFound();

            ViewModel = _mapper.Map<PersonFormViewModel>(person);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ViewModel.PersonId == null)
                ModelState.AddModelError("ViewModel", "Person details are required.");

            if (!ModelState.IsValid)
                return Page();

            var person = _mapper.Map<Person>(ViewModel);
            _context.Attach(person).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonExists(person.PersonId))
                    return NotFound();

                throw;
            }
            
            return RedirectToPage("/Persons/Index");
        }

        private bool PersonExists(int id)
        {
            return _context.Persons.Any(e => e.PersonId == id);
        }
    }
}