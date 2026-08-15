using BuildingRecordsApp.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BuildingRecordsApp.Pages.Properties;

public class DeleteModel(BuildingContext context) : PageModel
{
    [BindProperty] public Property Property { get; set; } = null!;
    public bool HasDependencies { get; private set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var property = await context.Properties.AsNoTracking().SingleOrDefaultAsync(p => p.PropertyId == id);
        if (property is null) return NotFound();
        Property = property;
        HasDependencies = await HasDependenciesAsync(id);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (Property is null) return NotFound();
        var property = await context.Properties.SingleOrDefaultAsync(p => p.PropertyId == Property.PropertyId);
        if (property is null) return NotFound();
        if (await HasDependenciesAsync(property.PropertyId))
        {
            Property = property;
            HasDependencies = true;
            ModelState.AddModelError(string.Empty, "This property still contains buildings, parking bays, or storerooms and cannot be deleted.");
            return Page();
        }
        context.Properties.Remove(property);
        await context.SaveChangesAsync();
        return RedirectToPage("./Index");
    }

    private Task<bool> HasDependenciesAsync(int id) => context.Properties
        .AnyAsync(p => p.PropertyId == id && (p.Buildings.Any() || p.ParkingBays.Any() || p.StoreRooms.Any()));
}
