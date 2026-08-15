using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;
using AutoMapper;

namespace BuildingRecordsApp.Pages.Owners
{
    public class EditModel : PageModel
    {
        private readonly BuildingContext _context;
        private readonly ISelectListService _selectListService;
        private readonly IMapper _mapper;

        public EditModel(BuildingContext context, ISelectListService selectListService, IMapper mapper)
        {
            _context = context;
            _selectListService = selectListService;
            _mapper = mapper;
        }
        [BindProperty]
        public required OwnerFormViewModel ViewModel { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            var owner = await _context.Owners
                .Include(o => o.Person)
                .Include(o => o.Ownership)
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.OwnershipContactId == id);

            if (owner == null)
                return NotFound();

            ViewModel = _mapper.Map<OwnerFormViewModel>(owner);
            ViewModel.OwnershipSelectList = await _selectListService.GetOwnershipSelectListAsync();
            ViewModel.PersonSelectList = await _selectListService.GetPersonSelectListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ViewModel.OwnershipContactId == null)
                ModelState.AddModelError("ViewModel", "OwnershipContact details are required.");
                
            if (ViewModel.PersonId == null)
                ModelState.AddModelError("ViewModel.PersonId", "Person is required.");
                
            if (ViewModel.OwnershipId == null)
                ModelState.AddModelError("ViewModel.OwnershipId", "Ownership is required.");

            if (!ModelState.IsValid)
                return Page();

            var owner = _mapper.Map<OwnershipContact>(ViewModel);
            _context.Attach(owner).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OwnerExists(owner.OwnershipContactId))
                    return NotFound();

                throw;
            }

            return RedirectToPage("/Owners/Index");
        }

        private bool OwnerExists(int id)
        {
            return _context.Owners.Any(e => e.OwnershipContactId == id);
        }
    }
}
