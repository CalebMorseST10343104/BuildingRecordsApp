using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;
using AutoMapper;

namespace BuildingRecordsApp.Pages.AccessDeviceCounts
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
        public required AccessDeviceCountFormViewModel ViewModel { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            var tagRemoteRecord = await _context.AccessDeviceCounts
                .Include(tr => tr.Unit)
                .AsNoTracking()
                .FirstOrDefaultAsync(tr => tr.AccessDeviceCountId == id);

            if (tagRemoteRecord == null)
                return NotFound();

            ViewModel = _mapper.Map<AccessDeviceCountFormViewModel>(tagRemoteRecord);
            ViewModel.UnitSelectList = await _selectListService.GetUnitSelectListAsync();
                
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ViewModel.AccessDeviceCountId == null)
                ModelState.AddModelError("ViewModel", "Tag Remote Record details are required.");
            
            if (ViewModel.UnitId == null)
                ModelState.AddModelError("ViewModel.UnitId", "Unit is required.");

            if (!ModelState.IsValid)
            {
                ViewModel.UnitSelectList = await _selectListService.GetUnitSelectListAsync();
                return Page();
            }

            var tagRemoteRecord = _mapper.Map<AccessDeviceCount>(ViewModel);
            _context.Attach(tagRemoteRecord).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccessDeviceCountExists(tagRemoteRecord.AccessDeviceCountId))
                    return NotFound();

                throw;
            }

            return RedirectToPage("/AccessDeviceCounts/Index");
        }

        private bool AccessDeviceCountExists(int id)
        {
            return _context.AccessDeviceCounts.Any(e => e.AccessDeviceCountId == id);
        }
    }
}
