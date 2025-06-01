using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;
using AutoMapper;
using BuildingRecordsApp.Models.DisplayViewModels;
using BuildingRecordsApp.Models.ItemViewModels;
using BuildingRecordsApp.Interfaces;

namespace BuildingRecordsApp.Pages.Ownerships
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
        public DisplayViewModel<OwnershipItemViewModel> ViewModel { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            var ownership = await _context.Ownerships
                .Include(o => o.Unit)
                .Include(o => o.CompanyTrust)
                .Include(o => o.Owners)
                .ThenInclude(w => w.Person)
                .FirstOrDefaultAsync(m => m.OwnershipId == id);

            if (ownership == null)
                return NotFound();

            ViewModel = new DisplayViewModel<OwnershipItemViewModel>
            {
                Entries = [_mapper.Map<OwnershipItemViewModel>(ownership)],
                IdsToDisplay = [ownership.OwnershipId],
                DisplayMode = Enums.DisplayMode.Detailed,
                DisplayLayout = Enums.DisplayLayout.List
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Ownerships == null)
                return NotFound();

            var ownership = await _context.Ownerships.FindAsync(id);

            if (ownership != null)
            {
                _context.Ownerships.Remove(ownership);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }

        public int GetFirstId()
        {
            if (ViewModel?.Entries != null && ViewModel.Entries.Count > 0)
            {
                return ViewModel.Entries[0].OwnershipId ?? 0;
            }
            return 0;
        }
    }
}
