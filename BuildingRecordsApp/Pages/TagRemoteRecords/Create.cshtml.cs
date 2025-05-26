using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BuildingRecordsApp.Enums;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;
using AutoMapper;

namespace BuildingRecordsApp.Pages.TagRemoteRecords
{
    public class CreateModel : PageModel
    {
        private readonly BuildingContext _context;
        private readonly ISelectListService _selectListService;
        private readonly IMapper _mapper;

        public CreateModel(BuildingContext context, ISelectListService selectListService, IMapper mapper)
        {
            _context = context;
            _selectListService = selectListService;
            _mapper = mapper;
        }

        [BindProperty]
        public TagRemoteRecordFormViewModel ViewModel { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            ViewModel = new TagRemoteRecordFormViewModel
            {
                UnitSelectList = await _selectListService.GetUnitSelectListAsync(UsageContext.ForTagRemoteRecord)
            };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ViewModel.UnitId == null)
            {
                ModelState.AddModelError("ViewModel.UnitId", "Unit is required.");
            }
            if (!ModelState.IsValid)
            {
                ViewModel.UnitSelectList = await _selectListService.GetUnitSelectListAsync(UsageContext.ForTagRemoteRecord);
                return Page();
            }

            var tagRemoteRecord = _mapper.Map<TagRemoteRecord>(ViewModel);

            _context.TagRemoteRecords.Add(tagRemoteRecord);
            await _context.SaveChangesAsync();

            return RedirectToPage("/TagRemoteRecords/Index");
        }
    }
}