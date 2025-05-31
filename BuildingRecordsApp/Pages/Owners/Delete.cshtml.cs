using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;
using AutoMapper;
using BuildingRecordsApp.Models.DisplayViewModels;
using BuildingRecordsApp.Models.ItemViewModels;

namespace BuildingRecordsApp.Pages.Owners
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
        public DisplayViewModel<OwnerItemViewModel> ViewModel { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Owner = await _context.Owners
                .Include(o => o.Person)
                .Include(o => o.Ownership)
                .ThenInclude(os => os!.Unit)
                .ThenInclude(u => u!.Building)
                .FirstOrDefaultAsync(m => m.OwnerId == id);

            if (Owner == null)
            {
                return NotFound();
            }
            ViewModel = new DisplayViewModel<OwnerItemViewModel>
            {
                Entries = [ _mapper.Map<OwnerItemViewModel>(Owner) ],
                IdsToDisplay = [ Owner.OwnerId ],
                DisplayMode = Enums.DisplayMode.Detailed,
                DisplayLayout = Enums.DisplayLayout.List
            };
            return Page();
        }
        
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Owner = await _context.Owners.FindAsync(id);

            if (Owner != null)
            {
                _context.Owners.Remove(Owner);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
