using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;
using AutoMapper;

namespace BuildingRecordsApp.Pages.Persons
{
    public class CreateModel : PageModel
    {
        private readonly BuildingContext _context;
        private readonly IMapper _mapper;

        public CreateModel(BuildingContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [BindProperty]
        public PersonFormViewModel ViewModel { get; set; } = new();

        public IActionResult OnGet()
        {
            ViewModel = new PersonFormViewModel();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var person = _mapper.Map<Person>(ViewModel);

            _context.Persons.Add(person);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Persons/Index");
        }
    }
}