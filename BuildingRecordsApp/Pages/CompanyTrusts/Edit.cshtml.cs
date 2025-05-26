using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;
using AutoMapper;

namespace BuildingRecordsApp.Pages.CompanyTrusts
{
    public class EditModel : PageModel
    {
        private readonly BuildingContext _context;
        private readonly IMapper _mapper;

        public EditModel(BuildingContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [BindProperty]
        public required CompanyTrustFormViewModel ViewModel { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            var companyTrust = await _context.CompanyTrusts
                .AsNoTracking()
                .FirstOrDefaultAsync(ct => ct.CompanyTrustId == id);

            if (companyTrust == null)
                return NotFound();

            ViewModel = _mapper.Map<CompanyTrustFormViewModel>(companyTrust);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {   
            if (ViewModel.CompanyTrustId == null)
                ModelState.AddModelError("ViewModel", "Company Trust details are required.");
            
            if (!ModelState.IsValid)
                return Page();

            var companyTrust = _mapper.Map<CompanyTrust>(ViewModel);
            _context.Attach(companyTrust).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompanyTrustExists(companyTrust.CompanyTrustId))
                    return NotFound();

                throw;
            }

            return RedirectToPage("/CompanyTrusts/Index");
        }

        private bool CompanyTrustExists(int id)
        {
            return _context.CompanyTrusts.Any(e => e.CompanyTrustId == id);
        }
    }
}
