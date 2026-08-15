using BuildingRecordsApp.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BuildingRecordsApp.Pages.Properties;

public class DetailsModel(BuildingContext context) : PageModel
{
    public Property Property { get; private set; } = null!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var property = await context.Properties
            .AsNoTracking()
            .AsSplitQuery()
            .Include(p => p.Buildings).ThenInclude(b => b.Units)
            .Include(p => p.ParkingBays).ThenInclude(b => b.Unit)
            .Include(p => p.StoreRooms).ThenInclude(s => s.Unit)
            .SingleOrDefaultAsync(p => p.PropertyId == id);
        if (property is null) return NotFound();
        Property = property;
        return Page();
    }
}
