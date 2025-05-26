using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;
using AutoMapper;

namespace BuildingRecordsApp.Pages.Ownerships
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
        public OwnershipFormViewModel ViewModel { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            ViewModel = new OwnershipFormViewModel
            {
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
                return Page();
            }

            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState is invalid");
                return Page();
            }

            var ownership = _mapper.Map<Ownership>(ViewModel);
            
            _context.Ownerships.Add(ownership);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Ownerships/Index");
        }
    }
}