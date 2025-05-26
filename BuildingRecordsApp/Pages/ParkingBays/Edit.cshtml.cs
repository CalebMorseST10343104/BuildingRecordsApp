using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;
using AutoMapper;

namespace BuildingRecordsApp.Pages.ParkingBays
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
        public required ParkingBayFormViewModel ViewModel { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            var parkingBay = await _context.ParkingBays
                .Include(pb => pb.Unit)
                .AsNoTracking()
                .FirstOrDefaultAsync(pb => pb.ParkingBayId == id);

            if (parkingBay == null)
                return NotFound();

            ViewModel = _mapper.Map<ParkingBayFormViewModel>(parkingBay);
            ViewModel.UnitSelectList = await _selectListService.GetUnitSelectListAsync();
                
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ViewModel.ParkingBayId == null)
                ModelState.AddModelError("ViewModel", "Parking Bay details are required.");

            if (!ModelState.IsValid)
            {
                ViewModel.UnitSelectList = await _selectListService.GetUnitSelectListAsync();
                return Page();
            }

            var parkingBay = _mapper.Map<ParkingBay>(ViewModel);
            _context.Attach(parkingBay).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ParkingBayExists(parkingBay.ParkingBayId))
                    return NotFound();

                throw;
            }

            return RedirectToPage("/ParkingBays/Index");
        }
        
        private bool ParkingBayExists(int id)
        {
            return _context.ParkingBays.Any(e => e.ParkingBayId == id);
        }
    }
}
