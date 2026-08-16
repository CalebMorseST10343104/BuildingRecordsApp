using BuildingRecordsApp.Models.FormViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BuildingRecordsApp.Pages.Properties;

public class EditModel(BuildingContext context) : PageModel
{
    [BindProperty] public PropertyFormViewModel ViewModel { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var property = await context.Properties.AsNoTracking().SingleOrDefaultAsync(p => p.PropertyId == id);
        if (property is null) return NotFound();
        ViewModel = new() { PropertyId = property.PropertyId, Name = property.Name, Address = property.Address };
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (ViewModel.PropertyId is null) return NotFound();
        ViewModel.Name = ViewModel.Name.Trim();
        if (await context.Properties.AnyAsync(p => p.Name == ViewModel.Name && p.PropertyId != ViewModel.PropertyId))
            ModelState.AddModelError("ViewModel.Name", "A property with this name already exists.");
        if (!ModelState.IsValid) return Page();

        var property = await context.Properties.SingleOrDefaultAsync(p => p.PropertyId == ViewModel.PropertyId);
        if (property is null) return NotFound();
        property.Name = ViewModel.Name;
        property.Address = ViewModel.Address?.Trim() ?? string.Empty;
        await context.SaveChangesAsync();
        return RedirectToPage("./Details", new { id = property.PropertyId });
    }
}
