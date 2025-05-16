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
                .ToListAsync();

            return new SelectList(units, "Id", u => $"[{u.Building.Name}] {u.UnitNumber}");
        }

        public async Task<SelectList> GetBuildingSelectListAsync()
        {
            var buildings = await _context.Buildings.ToListAsync();
            return new SelectList(buildings, "Id", "Name");
        }

        public async Task<SelectList> GetParkingBaySelectListAsync()
        {
            var parkingBays = await _context.ParkingBays
                .Include(pb => pb.Building)
                .ToListAsync();

            return new SelectList(parkingBays, "Id", pb => $"[{pb.Building.Name}] {pb.ParkingBayNumber}");
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
                .ToListAsync();

            return new SelectList(agents, "AgentId", a => $"{a.AgentCompany.CompanyName} - {a.FirstName} {a.LastName}");
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
                .ToListAsync();

            return new SelectList(ownerships, "Id", o => $"{o.Unit.Building.Name} - {o.Unit.UnitNumber}");
        }
    }
}