using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models;
using BuildingRecordsApp.ViewModels;

namespace BuildingRecordsApp.Pages.Owners
{
    public class EditModel : PageModel
    {
        private readonly BuildingContext _context;
        private readonly ISelectListService _selectListService;

        public EditModel(BuildingContext context, ISelectListService selectListService)
        {
            _context = context;
            _selectListService = selectListService;
        }
        [BindProperty]
        public required OwnerFormViewModel ViewModel { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            ViewModel = new OwnerFormViewModel
            {
                Owner = await _context.Owners
                    .Include(o => o.Person)
                    .Include(o => o.Ownership)
                    .FirstOrDefaultAsync(m => m.OwnerId == id) ?? null!,
                PersonSelectList = await _selectListService.GetPersonSelectListAsync(),
                OwnershipSelectList = await _selectListService.GetOwnershipSelectListAsync()
            };

            if (ViewModel.Owner == null)
                return NotFound();

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

            _context.Attach(ViewModel.Owner).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OwnerExists(ViewModel.Owner.OwnerId))
                    return NotFound();

                throw;
            }

            return RedirectToPage("/Owners/Index");
        }

        private bool OwnerExists(int id)
        {
            return _context.Owners.Any(e => e.OwnerId == id);
        }
    }
}
