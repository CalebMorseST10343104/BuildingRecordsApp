using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace BuildingRecordsApp.Pages.StoreRooms
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
        public StoreRoomFormViewModel ViewModel { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            ViewModel = new StoreRoomFormViewModel
            {
                PropertyId = await _context.Properties.Select(p => p.PropertyId).FirstAsync(),
                UnitSelectList = await _selectListService.GetUnitSelectListAsync()
            };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
 
            if (ViewModel.UnitId is int unitId)
            {
                var unitPropertyId = await _context.Units.Where(u => u.UnitId == unitId).Select(u => u.Building!.PropertyId).SingleOrDefaultAsync();
                if (unitPropertyId != ViewModel.PropertyId)
                    ModelState.AddModelError("ViewModel.UnitId", "The unit must be in the same property as the storeroom.");
            }
            if (!ModelState.IsValid)
            {
                ViewModel.UnitSelectList = await _selectListService.GetUnitSelectListAsync();
                return Page();
            }

            var storeRoom = _mapper.Map<StoreRoom>(ViewModel);

            _context.StoreRooms.Add(storeRoom);
            await _context.SaveChangesAsync();

            return RedirectToPage("/StoreRooms/Index");
        }
    }
}
