using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;
using AutoMapper;

namespace BuildingRecordsApp.Pages.TagRemoteRecords
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
        public required TagRemoteRecordFormViewModel ViewModel { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            var tagRemoteRecord = await _context.TagRemoteRecords
                .Include(tr => tr.Unit)
                .AsNoTracking()
                .FirstOrDefaultAsync(tr => tr.TagRemoteRecordId == id);

            if (tagRemoteRecord == null)
                return NotFound();

            ViewModel = _mapper.Map<TagRemoteRecordFormViewModel>(tagRemoteRecord);
            ViewModel.UnitSelectList = await _selectListService.GetUnitSelectListAsync();
                
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ViewModel.TagRemoteRecordId == null)
                ModelState.AddModelError("ViewModel", "Tag Remote Record details are required.");
            
            if (ViewModel.UnitId == null)
                ModelState.AddModelError("ViewModel.TagRemoteRecord.UnitId", "Unit is required.");

            if (!ModelState.IsValid)
            {
                ViewModel.UnitSelectList = await _selectListService.GetUnitSelectListAsync();
                return Page();
            }

            var tagRemoteRecord = _mapper.Map<TagRemoteRecord>(ViewModel);
            _context.Attach(tagRemoteRecord).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TagRemoteRecordExists(tagRemoteRecord.TagRemoteRecordId))
                    return NotFound();

                throw;
            }

            return RedirectToPage("/TagRemoteRecords/Index");
        }

        private bool TagRemoteRecordExists(int id)
        {
            return _context.TagRemoteRecords.Any(e => e.TagRemoteRecordId == id);
        }
    }
}
