using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;
using AutoMapper;
using BuildingRecordsApp.Services;
using Microsoft.EntityFrameworkCore;

namespace BuildingRecordsApp.Pages.OwnershipContacts
{
    public class CreateModel : PageModel
    {
        private readonly BuildingContext _context;
        private readonly ISelectListService _selectListService;
        private readonly IMapper _mapper;
        private readonly IOwnershipService _ownershipService;

        public CreateModel(BuildingContext context, ISelectListService selectListService, IMapper mapper, IOwnershipService ownershipService)
        {
            _context = context;
            _selectListService = selectListService;
            _mapper = mapper;
            _ownershipService = ownershipService;
        }

        [BindProperty]
        public OwnershipContactFormViewModel ViewModel { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int? ownershipId)
        {
            ViewModel = new OwnershipContactFormViewModel
            {
                OwnershipId = ownershipId,
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

            try
            {
                await _ownershipService.AddContactAsync(ViewModel.OwnershipId!.Value, ViewModel.PersonId!.Value);
            }
            catch (BusinessRuleException exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
                ViewModel.OwnershipSelectList = await _selectListService.GetOwnershipSelectListAsync();
                ViewModel.PersonSelectList = await _selectListService.GetPersonSelectListAsync();
                return Page();
            }

            var unitId = await _context.Ownerships.Where(o => o.OwnershipId == ViewModel.OwnershipId).Select(o => o.UnitId).SingleAsync();
            return RedirectToPage("/Units/Details", new { id = unitId });
        }
    }
}
