using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using BuildingRecordsApp.Models;
using BuildingRecordsApp.Enums;

namespace BuildingRecordsApp.Services
{
    public class SelectListService : ISelectListService
    {
        private readonly BuildingContext _context;

        public SelectListService(BuildingContext context)
        {
            _context = context;
        }

        public async Task<SelectList> GetUnitSelectListAsync()
        {
            return await GetUnitSelectListAsync(UsageContext.All);
        }

        public async Task<SelectList> GetUnitSelectListAsync(UsageContext usageContext)
        {
            IQueryable<Unit> query = _context.Units;

            var contextUnitSelectors = new Dictionary<UsageContext, Func<Task<List<int?>>>>
            {
                [UsageContext.ForTagRemoteRecord] = () => _context.TagRemoteRecords.Select(tr => tr.UnitId).ToListAsync(),
                [UsageContext.ForLease] = () => _context.Leases.Select(l => l.UnitId).ToListAsync(),
                [UsageContext.ForOccupancy] = () => _context.Occupancies.Select(o => o.UnitId).ToListAsync(),
                [UsageContext.ForOwnership] = () => _context.Ownerships.Select(o => o.UnitId).ToListAsync()
            };

            if (contextUnitSelectors.TryGetValue(usageContext, out var selector))
            {
                var usedIn = await selector();
                query = query.Where(u => !usedIn.Contains(u.UnitId));
            }

            var units = await query
                .Include(u => u.Building)
                .Select(u => new
                {
                    u.UnitId,
                    Display = $"[{u.Building!.Name}] {u.UnitNumber}"
                })
                .ToListAsync();

            units.Insert(0, new { UnitId = 0, Display = "-- Select Unit --" });

            return new SelectList(units, "UnitId", "Display");
        }


        public async Task<SelectList> GetBuildingSelectListAsync()
        {
            var buildings = await _context.Buildings.ToListAsync();

            buildings.Insert(0, new Building
            {
                BuildingId = 0,
                Name = "-- Select Building --"
            });

            return new SelectList(buildings, "BuildingId", "Name");
        }

        public async Task<SelectList> GetParkingBaySelectListAsync()
        {
            var parkingBays = await _context.ParkingBays
                .Include(pb => pb.Unit!.Building)
                .Select(pb => new
                {
                    pb.ParkingBayID,
                    Display = $"[{pb.Unit!.Building!.Name}] {pb.ParkingBayNumber}"
                })
                .ToListAsync();

            return new SelectList(parkingBays, "ParkingBayID", "Display");
        }

        public async Task<SelectList> GetAgentCompanySelectListAsync()
        {
            var agentCompanies = await _context.AgentCompanies.ToListAsync();

            agentCompanies.Insert(0, new AgentCompany
            {
                AgentCompanyId = 0,
                CompanyName = "-- Select Agent Company --"
            });

            return new SelectList(agentCompanies, "AgentCompanyId", "CompanyName");
        }

        public async Task<SelectList> GetAgentSelectListAsync()
        {
            var agents = await _context.Agents
                .Include(a => a.AgentCompany)
                .Select(a => new
                {
                    a.AgentId,
                    Display = $"{a.AgentCompany!.CompanyName} - {a.FirstName} {a.LastName}"
                })
                .ToListAsync();

            return new SelectList(agents, "AgentId", "Display");
        }

        public async Task<SelectList> GetPersonSelectListAsync()
        {
            var persons = await _context.Persons
            .Select(p => new
            {
                p.PersonId,
                Display = $"{p.Name} {p.Surname}"
            })
            .ToListAsync();

            return new SelectList(persons, "PersonId", "Display");
        }

        public async Task<SelectList> GetOwnershipSelectListAsync()
        {
            var ownerships = await _context.Ownerships
                .Include(o => o.Unit)
                .Select(o => new
                {
                    o.OwnershipId,
                    Display = $"{o.Unit!.Building!.Name} - {o.Unit.UnitNumber}"
                })
                .ToListAsync();

            return new SelectList(ownerships, "OwnershipId", "Display");
        }

        public async Task<SelectList> GetCompanyTrustSelectListAsync()
        {
            var companyTrusts = await _context.CompanyTrusts.ToListAsync();
            return new SelectList(companyTrusts, "CompanyTrustId", "Name");
        }
    }
}