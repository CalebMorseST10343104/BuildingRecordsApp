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
            ViewModel.UnitSelectList = await _selectListService.GetUnitSelectListAsync(Enums.UsageContext.ForLease);

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
                ViewModel.UnitSelectList = await _selectListService.GetUnitSelectListAsync(Enums.UsageContext.ForLease);
                return Page();
            }

            var lease = _mapper.Map<Lease>(ViewModel);
            _context.Attach(lease).State = EntityState.Modified;

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

            return RedirectToPage("/Leases/Index");
        }

        private bool LeaseExists(int id)
        {
            return _context.Leases.Any(e => e.LeaseId == id);
        }
    }
}
