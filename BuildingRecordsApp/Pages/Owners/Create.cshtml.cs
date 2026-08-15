using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;
using AutoMapper;

namespace BuildingRecordsApp.Pages.Owners
{
    public class CreateModel : PageModel
    {
        private readonly BuildingContext _context;
        private readonly ISelectListService _selectListService;
        private readonly IMapper _mapper;

        public CreateModel(BuildingContext context, ISelectListService selectListService, IMapper mapper)
        {
            _context = context;
            _selectListService = selectListService;
            _mapper = mapper;
        }

        [BindProperty]
        public OwnerFormViewModel ViewModel { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            ViewModel = new OwnerFormViewModel
            {
                OwnershipSelectList = await _selectListService.GetOwnershipSelectListAsync(),
                PersonSelectList = await _selectListService.GetPersonSelectListAsync()
            };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {   
            if (ViewModel.PersonId == null)
                ModelState.AddModelError("ViewModel.PersonId", "Person is required.");
            
            if (ViewModel.OwnershipId == null)
                ModelState.AddModelError("ViewModel.OwnershipId", "Ownership is required.");
            
            if (!ModelState.IsValid)
            {
                ViewModel.OwnershipSelectList = await _selectListService.GetOwnershipSelectListAsync();
                ViewModel.PersonSelectList = await _selectListService.GetPersonSelectListAsync();
                return Page();
            }

            var owner = _mapper.Map<OwnershipContact>(ViewModel);

            _context.Owners.Add(owner);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Owners/Index");
        }
    }
}