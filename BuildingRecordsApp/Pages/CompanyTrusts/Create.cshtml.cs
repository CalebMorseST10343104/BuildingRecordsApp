using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;
using AutoMapper;

namespace BuildingRecordsApp.Pages.CompanyTrusts
{
    public class CreateModel : PageModel
    {
        private readonly BuildingContext _context;
        private readonly IMapper _mapper;

        public CreateModel(BuildingContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [BindProperty]
        public CompanyTrustFormViewModel ViewModel { get; set; } = new();

        public IActionResult OnGet()
        {
            ViewModel = new CompanyTrustFormViewModel();
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var companyTrust = _mapper.Map<CompanyTrust>(ViewModel);

            _context.CompanyTrusts.Add(companyTrust);
            await _context.SaveChangesAsync();

            return RedirectToPage("/CompanyTrusts/Index");
        }
    }
}