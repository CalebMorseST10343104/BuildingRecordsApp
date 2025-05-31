using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace BuildingRecordsApp.Pages.ParkingBays
{
    public class DeleteModel : PageModel
    {
        private readonly BuildingContext _context;
        private readonly IMapper _mapper;

        public DeleteModel(BuildingContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [BindProperty]
        public ParkingBay ParkingBay { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parkingBay = await _context.ParkingBays
                .Include(p => p.Unit)
                .ThenInclude(u => u!.Building)
                .FirstOrDefaultAsync(m => m.ParkingBayId == id);

            if (parkingBay == null)
            {
                return NotFound();
            }
            ParkingBay = parkingBay;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parkingBay = await _context.ParkingBays.FindAsync(id);

            if (parkingBay != null)
            {
                _context.ParkingBays.Remove(parkingBay);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
