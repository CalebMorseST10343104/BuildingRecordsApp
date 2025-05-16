using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using BuildingRecordsApp.Models;

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
            var units = await _context.Units
                .Include(u => u.Building)
                .Select(u => new
                {
                    u.UnitId,
                    Display = $"[{u.Building.Name}] {u.UnitNumber}"
                })
                .ToListAsync();

            return new SelectList(units, "Id", "Display");
        }

        public async Task<SelectList> GetBuildingSelectListAsync()
        {
            var buildings = await _context.Buildings.ToListAsync();
            return new SelectList(buildings, "Id", "Name");
        }

        public async Task<SelectList> GetParkingBaySelectListAsync()
        {
            var parkingBays = await _context.ParkingBays
                .Include(pb => pb.Unit.Building)
                .Select(pb => new
                {
                    pb.ParkingBayID,
                    Display = $"[{pb.Unit.Building.Name}] {pb.ParkingBayNumber}"
                })
                .ToListAsync();

            return new SelectList(parkingBays, "Id", "Display");
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
                    Display = $"{a.AgentCompany.CompanyName} - {a.FirstName} {a.LastName}"
                })
                .ToListAsync();

            return new SelectList(agents, "Id", "Display");
        }

        public async Task<SelectList> GetPersonSelectListAsync()
        {
            var persons = await _context.Persons.ToListAsync();
            return new SelectList(persons, "PersonId", "Name", "Surname");
        }

        public async Task<SelectList> GetOwnershipSelectListAsync()
        {
            var ownerships = await _context.Ownerships
                .Include(o => o.Unit)
                .Select(o => new
                {
                    o.OwnershipId,
                    Display = $"{o.Unit.Building.Name} - {o.Unit.UnitNumber}"
                })
                .ToListAsync();

            return new SelectList(ownerships, "Id", "Display");
        }
    }
}