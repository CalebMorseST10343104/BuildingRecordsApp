using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;
using AutoMapper;

namespace BuildingRecordsApp.Pages.Ownerships
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
        public required OwnershipFormViewModel ViewModel { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            var ownership = await _context.Ownerships
                .Include(o => o.Unit)
                .Include(o => o.Organization)
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.OwnershipId == id);

            if (ownership == null)
                return NotFound();

            ViewModel = _mapper.Map<OwnershipFormViewModel>(ownership);
            ViewModel.UnitSelectList = await _selectListService.GetUnitSelectListAsync(Enums.UsageContext.ForOwnership);
            ViewModel.CompanyTrustSelectList = await _selectListService.GetCompanyTrustSelectListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ViewModel.OwnershipId == null)
                ModelState.AddModelError("ViewModel", "Ownership details are required.");
            
            if (ViewModel.UnitId == null)
                ModelState.AddModelError("Ownership.UnitId", "Unit is required.");

            if (!ModelState.IsValid)
            {
                ViewModel.UnitSelectList = await _selectListService.GetUnitSelectListAsync(Enums.UsageContext.ForOwnership);
                ViewModel.CompanyTrustSelectList = await _selectListService.GetCompanyTrustSelectListAsync();
                return Page();
            }

            var ownership = _mapper.Map<Ownership>(ViewModel);
            _context.Attach(ownership).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OwnershipExists(ownership.OwnershipId))
                    return NotFound();

                throw;
            }

            return RedirectToPage("/Ownerships/Index");
        }

        private bool OwnershipExists(int id)
        {
            return _context.Ownerships.Any(e => e.OwnershipId == id);
        }
    }
}
