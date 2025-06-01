using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BuildingRecordsApp.Models.Entities;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using BuildingRecordsApp.Models.DisplayViewModels;
using BuildingRecordsApp.Models.ItemViewModels;
using BuildingRecordsApp.Interfaces;

namespace BuildingRecordsApp.Pages.TagRemoteRecords
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
        public DisplayViewModel<TagRemoteRecordItemViewModel> ViewModel { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            var tagRemoteRecord = await _context.TagRemoteRecords
                .Include(t => t.Unit)
                .ThenInclude(u => u!.Building)
                .FirstOrDefaultAsync(m => m.TagRemoteRecordId == id);

            if (tagRemoteRecord == null)
                return NotFound();

            ViewModel = new DisplayViewModel<TagRemoteRecordItemViewModel>
            {
                Entries = [_mapper.Map<TagRemoteRecordItemViewModel>(tagRemoteRecord)],
                IdsToDisplay = [tagRemoteRecord.TagRemoteRecordId],
                DisplayMode = Enums.DisplayMode.Extended,
                DisplayLayout = Enums.DisplayLayout.List
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.TagRemoteRecords == null)
                return NotFound();

            var tagRemoteRecord = await _context.TagRemoteRecords.FindAsync(id);

            if (tagRemoteRecord != null)
            {
                _context.TagRemoteRecords.Remove(tagRemoteRecord);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }

        public int GetFirstId()
        {
            if (ViewModel?.Entries != null && ViewModel.Entries.Count > 0)
            {
                return ViewModel.Entries[0].TagRemoteRecordId ?? 0;
            }
            return 0;
        }
    }
}
