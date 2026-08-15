using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;
using AutoMapper;
using BuildingRecordsApp.Services;

namespace BuildingRecordsApp.Pages.Ownerships
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
        public OwnershipFormViewModel ViewModel { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int? unitId)
        {
            ViewModel = new OwnershipFormViewModel
            {
                UnitId = unitId,
                UnitSelectList = await _selectListService.GetUnitSelectListAsync(Enums.UsageContext.ForOwnership),
                CompanyTrustSelectList = await _selectListService.GetCompanyTrustSelectListAsync()
            };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ViewModel.UnitId == null)
            {
                ModelState.AddModelError("ViewModel.UnitId", "Unit is required.");
            }

            if (!ModelState.IsValid)
            {
                ViewModel.UnitSelectList = await _selectListService.GetUnitSelectListAsync(Enums.UsageContext.ForOwnership);
                ViewModel.CompanyTrustSelectList = await _selectListService.GetCompanyTrustSelectListAsync();
                return Page();
            }

            try
            {
                await _ownershipService.SetOwnershipAsync(ViewModel.UnitId.GetValueOrDefault(), ViewModel.OwnershipType ?? string.Empty, ViewModel.OrganizationId);
            }
            catch (BusinessRuleException exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
                ViewModel.UnitSelectList = await _selectListService.GetUnitSelectListAsync(Enums.UsageContext.ForOwnership);
                ViewModel.CompanyTrustSelectList = await _selectListService.GetCompanyTrustSelectListAsync();
                return Page();
            }

            return RedirectToPage("/Units/Details", new { id = ViewModel.UnitId });
        }
    }
}
