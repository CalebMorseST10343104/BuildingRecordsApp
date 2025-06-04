using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models.Entities;
using AutoMapper;
using BuildingRecordsApp.Models.DisplayViewModels;
using BuildingRecordsApp.Models.ItemViewModels;
using BuildingRecordsApp.Interfaces;

namespace BuildingRecordsApp.Pages.CompanyTrusts
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
        public DisplayViewModel<CompanyTrustItemViewEntry> ViewModel { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            var companyTrust = await _context.CompanyTrusts
                .Include(c => c.Ownerships)
                .ThenInclude(o => o.Unit)
                .ThenInclude(u => u!.Building)
                .FirstOrDefaultAsync(m => m.CompanyTrustId == id);

            if (companyTrust == null)
                return NotFound();

            ViewModel = new DisplayViewModel<CompanyTrustItemViewEntry>
            {
                Entries = [_mapper.Map<CompanyTrustItemViewEntry>(companyTrust)],
                IdsToDisplay = [companyTrust.CompanyTrustId],
                DisplayMode = Enums.DisplayMode.Detailed,
                DisplayLayout = Enums.DisplayLayout.List
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.CompanyTrusts == null)
                return NotFound();

            var companyTrust = await _context.CompanyTrusts.FindAsync(id);

            if (companyTrust != null)
            {
                _context.CompanyTrusts.Remove(companyTrust);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }

        public int GetFirstId()
        {
            if (ViewModel?.Entries != null && ViewModel.Entries.Count > 0)
            {
                return ViewModel.Entries[0].CompanyTrustId ?? 0;
            }
            return 0;
        }
    }
}
