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

namespace BuildingRecordsApp.Pages.OwnershipContacts
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
        public DisplayViewModel<OwnershipContactItemViewEntry> ViewModel { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            var OwnershipContact = await _context.OwnershipContacts
                .Include(o => o.Person)
                .Include(o => o.Ownership)
                .ThenInclude(os => os!.Unit)
                .ThenInclude(u => u!.Building)
                .FirstOrDefaultAsync(m => m.OwnershipContactId == id);

            if (OwnershipContact == null)
                return NotFound();

            ViewModel = new DisplayViewModel<OwnershipContactItemViewEntry>
            {
                Entries = [_mapper.Map<OwnershipContactItemViewEntry>(OwnershipContact)],
                IdsToDisplay = [OwnershipContact.OwnershipContactId],
                DisplayMode = Enums.DisplayMode.Detailed,
                DisplayLayout = Enums.DisplayLayout.List
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.OwnershipContacts == null)
                return NotFound();

            var OwnershipContact = await _context.OwnershipContacts.FindAsync(id);

            if (OwnershipContact != null)
            {
                _context.OwnershipContacts.Remove(OwnershipContact);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }

        public int GetFirstId()
        {
            if (ViewModel?.Entries != null && ViewModel.Entries.Count > 0)
            {
                return ViewModel.Entries[0].OwnershipContactId ?? 0;
            }
            return 0;
        }
    }
}
