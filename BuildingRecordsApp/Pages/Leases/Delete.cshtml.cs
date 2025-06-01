using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models.Entities;
using AutoMapper;
using BuildingRecordsApp.Models.DisplayViewModels;
using BuildingRecordsApp.Models.ItemViewModels;
using BuildingRecordsApp.Interfaces;

namespace BuildingRecordsApp.Pages.Leases
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
        public DisplayViewModel<LeaseItemViewModel> ViewModel { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            var lease = await _context.Leases
                .Include(l => l.Unit)
                .ThenInclude(u => u!.Building)
                .FirstOrDefaultAsync(m => m.LeaseId == id);

            if (lease == null)
                return NotFound();

            ViewModel = new DisplayViewModel<LeaseItemViewModel>
            {
                Entries = [_mapper.Map<LeaseItemViewModel>(lease)],
                IdsToDisplay = [lease.LeaseId],
                DisplayMode = Enums.DisplayMode.Detailed,
                DisplayLayout = Enums.DisplayLayout.List
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Leases == null)
                return NotFound();

            var lease = await _context.Leases.FindAsync(id);

            if (lease != null)
            {
                _context.Leases.Remove(lease);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }

        public int GetFirstId()
        {
            if (ViewModel?.Entries != null && ViewModel.Entries.Count > 0)
            {
                return ViewModel.Entries[0].LeaseId ?? 0;
            }
            return 0; // Default value if no entries are present
        }
    }
}
