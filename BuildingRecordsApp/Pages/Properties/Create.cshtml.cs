using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BuildingRecordsApp.Pages.Properties;

public class CreateModel(BuildingContext context) : PageModel
{
    [BindProperty] public PropertyFormViewModel ViewModel { get; set; } = new();

    public async Task<IActionResult> OnPostAsync()
    {
        ViewModel.Name = ViewModel.Name.Trim();
        if (await context.Properties.AnyAsync(p => p.Name == ViewModel.Name))
            ModelState.AddModelError("ViewModel.Name", "A property with this name already exists.");
        if (!ModelState.IsValid)
            return Page();

        var property = new Property { Name = ViewModel.Name, Address = ViewModel.Address.Trim() };
        context.Properties.Add(property);
        await context.SaveChangesAsync();
        return RedirectToPage("./Details", new { id = property.PropertyId });
    }
}
