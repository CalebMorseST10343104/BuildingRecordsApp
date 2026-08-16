using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BuildingRecordsApp.Models.Entities;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using BuildingRecordsApp.Models.DisplayViewModels;
using BuildingRecordsApp.Models.ItemViewModels;
using BuildingRecordsApp.Interfaces;

namespace BuildingRecordsApp.Pages.AccessDeviceCounts
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
        public DisplayViewModel<AccessDeviceCountItemViewEntry> ViewModel { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            var tagRemoteRecord = await _context.AccessDeviceCounts
                .Include(t => t.Unit)
                .ThenInclude(u => u!.Building)
                .FirstOrDefaultAsync(m => m.AccessDeviceCountId == id);

            if (tagRemoteRecord == null)
                return NotFound();

            ViewModel = new DisplayViewModel<AccessDeviceCountItemViewEntry>
            {
                Entries = [_mapper.Map<AccessDeviceCountItemViewEntry>(tagRemoteRecord)],
                IdsToDisplay = [tagRemoteRecord.AccessDeviceCountId],
                DisplayMode = Enums.DisplayMode.Extended,
                DisplayLayout = Enums.DisplayLayout.List
            };

            return Page();
        }

        public IActionResult OnPost(int? id)
        {
            return RedirectToPage("./Delete", new { id });
        }

        public int GetFirstId()
        {
            if (ViewModel?.Entries != null && ViewModel.Entries.Count > 0)
            {
                return ViewModel.Entries[0].AccessDeviceCountId ?? 0;
            }
            return 0;
        }
    }
}
