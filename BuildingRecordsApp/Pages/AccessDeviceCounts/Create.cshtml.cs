using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BuildingRecordsApp.Enums;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace BuildingRecordsApp.Pages.AccessDeviceCounts
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
        public AccessDeviceCountFormViewModel ViewModel { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            ViewModel = new AccessDeviceCountFormViewModel
            {
                UnitSelectList = await _selectListService.GetUnitSelectListAsync(UsageContext.ForAccessDeviceCount)
            };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ViewModel.UnitId == null)
            {
                ModelState.AddModelError("ViewModel.UnitId", "Unit is required.");
            }
            if (ViewModel.UnitId is int unitId && await _context.AccessDeviceCounts.AnyAsync(a => a.UnitId == unitId))
                ModelState.AddModelError("ViewModel.UnitId", "This unit already has an access-device count record. Edit the existing record instead.");
            if (!ModelState.IsValid)
            {
                ViewModel.UnitSelectList = await _selectListService.GetUnitSelectListAsync(UsageContext.ForAccessDeviceCount);
                return Page();
            }

            var accessDeviceCount = _mapper.Map<AccessDeviceCount>(ViewModel);

            _context.AccessDeviceCounts.Add(accessDeviceCount);
            await _context.SaveChangesAsync();

            return RedirectToPage("/AccessDeviceCounts/Index");
        }
    }
}
