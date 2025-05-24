using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BuildingRecordsApp.Models;
using BuildingRecordsApp.ViewModels;

namespace BuildingRecordsApp.Pages.Owners
{
    public class CreateModel : PageModel
    {
        private readonly BuildingContext _context;
        private readonly ISelectListService _selectListService;

        public CreateModel(BuildingContext context, ISelectListService selectListService)
        {
            _context = context;
            _selectListService = selectListService;
        }

        [BindProperty]
        public OwnerFormViewModel ViewModel { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            ViewModel = new OwnerFormViewModel
            {
                Owner = new Owner(),
                OwnershipSelectList = await _selectListService.GetOwnershipSelectListAsync(),
                PersonSelectList = await _selectListService.GetPersonSelectListAsync()
            };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ViewModel == null)
            {
                ModelState.AddModelError("ViewModel", "Owner details are required.");
                return Page();
            }
            if (ViewModel.Owner == null)
            {
                ModelState.AddModelError("ViewModel.Owner", "Owner details are required.");
                return Page();
            }
            if (ViewModel.Owner.PersonId == null)
            {
                ModelState.AddModelError("ViewModel.Owner.PersonId", "Person is required.");
                return Page();
            }
            if (ViewModel.Owner.OwnershipId == null)
            {
                ModelState.AddModelError("ViewModel.Owner.OwnershipId", "Ownership is required.");
                return Page();
            }
            if (!ModelState.IsValid)
                return Page();

            _context.Owners.Add(ViewModel.Owner);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Owners/Index");
        }
    }
}