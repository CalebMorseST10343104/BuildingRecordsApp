using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;
using AutoMapper;

namespace BuildingRecordsApp.Pages.Leases
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
        public required LeaseFormViewModel ViewModel { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            var lease = await _context.Leases
                .Include(l => l.Unit)
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.LeaseId == id);

            if (lease == null)
                return NotFound();

            ViewModel = _mapper.Map<LeaseFormViewModel>(lease);
            ViewModel.UnitSelectList = await _selectListService.GetUnitSelectListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ViewModel.LeaseId == null)
                ModelState.AddModelError("ViewModel", "Lease details are required.");
            
            if (ViewModel.UnitId == null)
                ModelState.AddModelError("ViewModel.UnitId", "Unit is required.");

            if (!ModelState.IsValid)
            {
                ViewModel.UnitSelectList = await _selectListService.GetUnitSelectListAsync();
                return Page();
            }

            var lease = await _context.Leases.SingleOrDefaultAsync(l => l.LeaseId == ViewModel.LeaseId);
            if (lease is null)
                return NotFound();
            if (lease.UnitId != ViewModel.UnitId)
            {
                ModelState.AddModelError("ViewModel.UnitId", "A lease summary cannot be moved to another unit.");
                ViewModel.UnitId = lease.UnitId;
                ViewModel.UnitSelectList = await _selectListService.GetUnitSelectListAsync();
                return Page();
            }

            _mapper.Map(ViewModel, lease);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LeaseExists(lease.LeaseId))
                    return NotFound();

                throw;
            }

            return RedirectToPage("/Units/Details", new { id = lease.UnitId });
        }

        private bool LeaseExists(int id)
        {
            return _context.Leases.Any(e => e.LeaseId == id);
        }
    }
}
