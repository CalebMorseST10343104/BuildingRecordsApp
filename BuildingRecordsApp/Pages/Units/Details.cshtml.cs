using BuildingRecordsApp.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BuildingRecordsApp.Pages.Units;

public class DetailsModel(BuildingContext context) : PageModel
{
    public Unit Unit { get; private set; } = null!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var unit = await context.Units
            .AsNoTracking()
            .AsSplitQuery()
            .Include(u => u.Building).ThenInclude(b => b!.Property)
            .Include(u => u.PrimaryContactPerson)
            .Include(u => u.Agent).ThenInclude(a => a!.Person)
            .Include(u => u.Agent).ThenInclude(a => a!.AgentCompany)
            .Include(u => u.Ownership).ThenInclude(o => o!.Organization)
            .Include(u => u.Ownership).ThenInclude(o => o!.OwnershipContacts).ThenInclude(c => c.Person)
            .Include(u => u.Occupants).ThenInclude(o => o.Occupant)
            .Include(u => u.Lease)
            .Include(u => u.Vehicles)
            .Include(u => u.ParkingBays)
            .Include(u => u.StoreRooms)
            .Include(u => u.AccessDeviceCount)
            .SingleOrDefaultAsync(u => u.UnitId == id);
        if (unit is null) return NotFound();
        Unit = unit;
        return Page();
    }
}
