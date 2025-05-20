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

            var usedIn = new List<int?>();
            switch (usageContext)
            {
                case UsageContext.ForTagRemoteRecord:
                    usedIn = await _context.TagRemoteRecords
                    .Select(tr => tr.UnitId)
                    .ToListAsync();
                    query = query.Where(u => !usedIn.Contains(u.UnitId));
                    break;
                case UsageContext.ForLease:
                    usedIn = await _context.Leases
                    .Select(l => l.UnitId)
                    .ToListAsync();
                    query = query.Where(u => !usedIn.Contains(u.UnitId));
                    break;
                case UsageContext.ForOccupancy:
                    usedIn = await _context.Occupancies
                    .Select(o => o.UnitId)
                    .ToListAsync();
                    query = query.Where(u => !usedIn.Contains(u.UnitId));
                    break;
                case UsageContext.All:
                default:
                    break;
            }

            var units = await query
                .Include(u => u.Building)
                .Select(u => new
                {
                    u.UnitId,
                    Display = $"[{u.Building!.Name}] {u.UnitNumber}"
                })
                .ToListAsync();

            return new SelectList(units, "UnitId", "Display");
        }

        public async Task<SelectList> GetBuildingSelectListAsync()
        {
            var buildings = await _context.Buildings.ToListAsync();
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
    }
}