using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;
using AutoMapper;

namespace BuildingRecordsApp.Pages.Leases
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
        public LeaseFormViewModel ViewModel { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int? unitId)
        {
            ViewModel = new LeaseFormViewModel
            {
                UnitId = unitId,
                UnitSelectList = await _selectListService.GetUnitSelectListAsync(Enums.UsageContext.ForLease)
            };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ViewModel.UnitId == null)
            {
                ModelState.AddModelError("Lease.UnitId", "Unit is required.");
            }
            
            if (!ModelState.IsValid)
            {
                ViewModel.UnitSelectList = await _selectListService.GetUnitSelectListAsync(Enums.UsageContext.ForLease);
                return Page();
            }

            var lease = _mapper.Map<Lease>(ViewModel);

            _context.Leases.Add(lease);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Units/Details", new { id = ViewModel.UnitId });
        }
    }
}
