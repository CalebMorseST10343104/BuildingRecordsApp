using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;
using AutoMapper;
using BuildingRecordsApp.Services;

namespace BuildingRecordsApp.Pages.Ownerships
{
    public class EditModel : PageModel
    {
        private readonly BuildingContext _context;
        private readonly ISelectListService _selectListService;
        private readonly IMapper _mapper;
        private readonly IOwnershipService _ownershipService;

        public EditModel(BuildingContext context, ISelectListService selectListService, IMapper mapper, IOwnershipService ownershipService)
        {
            _context = context;
            _selectListService = selectListService;
            _mapper = mapper;
            _ownershipService = ownershipService;
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
            ViewModel.OrganizationSelectList = await _selectListService.GetOrganizationSelectListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ViewModel.OwnershipId == null)
                ModelState.AddModelError("ViewModel", "Ownership details are required.");
            
            if (ViewModel.UnitId == null)
                ModelState.AddModelError("ViewModel.UnitId", "Unit is required.");

            if (!ModelState.IsValid)
            {
                ViewModel.UnitSelectList = await _selectListService.GetUnitSelectListAsync(Enums.UsageContext.ForOwnership);
                ViewModel.OrganizationSelectList = await _selectListService.GetOrganizationSelectListAsync();
                return Page();
            }

            try
            {
                var existing = await _context.Ownerships.AsNoTracking()
                    .SingleOrDefaultAsync(o => o.OwnershipId == ViewModel.OwnershipId.GetValueOrDefault());
                if (existing is null)
                    return NotFound();
                if (existing.UnitId != ViewModel.UnitId.GetValueOrDefault())
                    throw new BusinessRuleException("An ownership cannot be moved to another unit.");

                await _ownershipService.SetOwnershipAsync(
                    ViewModel.UnitId.GetValueOrDefault(),
                    ViewModel.OwnershipType ?? string.Empty,
                    ViewModel.OrganizationId);
            }
            catch (BusinessRuleException exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
                ViewModel.UnitSelectList = await _selectListService.GetUnitSelectListAsync(Enums.UsageContext.ForOwnership);
                ViewModel.OrganizationSelectList = await _selectListService.GetOrganizationSelectListAsync();
                return Page();
            }

            return RedirectToPage("/Ownerships/Index");
        }

        private bool OwnershipExists(int id)
        {
            return _context.Ownerships.Any(e => e.OwnershipId == id);
        }
    }
}
