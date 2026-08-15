using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BuildingRecordsApp.Pages.Properties;

public class IndexModel(BuildingContext context) : PageModel
{
    public List<PropertySummary> Properties { get; private set; } = [];

    public async Task OnGetAsync() => Properties = await context.Properties
        .AsNoTracking()
        .OrderBy(p => p.Name)
        .Select(p => new PropertySummary(
            p.PropertyId,
            p.Name,
            p.Address,
            p.Buildings.Count,
            p.Buildings.SelectMany(b => b.Units).Count(),
            p.ParkingBays.Count + p.StoreRooms.Count))
        .ToListAsync();

    public sealed record PropertySummary(int PropertyId, string Name, string Address, int BuildingCount, int UnitCount, int InfrastructureCount);
}
