using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using BuildingRecordsApp.Models.DisplayViewModels;
using BuildingRecordsApp.Models.ItemViewModels;
using BuildingRecordsApp.Interfaces;

namespace BuildingRecordsApp.Pages.ParkingBays
{
    public class DeleteModel : PageModel, ISingleDisplay
    {
        private readonly BuildingContext _context;
        private readonly IMapper _mapper;

        public DeleteModel(BuildingContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [BindProperty]
        public DisplayViewModel<ParkingBayItemViewModel> ViewModel { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            var parkingBay = await _context.ParkingBays
                .Include(p => p.Unit)
                .ThenInclude(u => u!.Building)
                .FirstOrDefaultAsync(m => m.ParkingBayId == id);

            if (parkingBay == null)
                return NotFound();

            ViewModel = new DisplayViewModel<ParkingBayItemViewModel>
            {
                Entries = [_mapper.Map<ParkingBayItemViewModel>(parkingBay)],
                IdsToDisplay = [parkingBay.ParkingBayId],
                DisplayMode = Enums.DisplayMode.Detailed,
                DisplayLayout = Enums.DisplayLayout.List
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.ParkingBays == null)
                return NotFound();

            var parkingBay = await _context.ParkingBays.FindAsync(id);

            if (parkingBay != null)
            {
                _context.ParkingBays.Remove(parkingBay);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }

        public int GetFirstId()
        {
            if (ViewModel?.Entries != null && ViewModel.Entries.Count > 0)
            {
                return ViewModel.Entries[0].ParkingBayId ?? 0;
            }
            return 0;
        }
    }
}
