using BuildingRecordsApp.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BuildingRecordsApp.Services;

public sealed class AgentService(BuildingContext context) : IAgentService
{
    public async Task<Agent> CreateProfileAsync(int personId, int agentCompanyId, CancellationToken cancellationToken = default)
    {
        if (!await context.Persons.AnyAsync(p => p.PersonId == personId, cancellationToken))
            throw new BusinessRuleException("Person not found.");
        if (!await context.AgentCompanies.AnyAsync(c => c.AgentCompanyId == agentCompanyId, cancellationToken))
            throw new BusinessRuleException("Agent company not found.");
        if (await context.Agents.AnyAsync(a => a.PersonId == personId, cancellationToken))
            throw new BusinessRuleException("This person already has an agent profile.");

        var agent = new Agent { PersonId = personId, AgentCompanyId = agentCompanyId };
        context.Agents.Add(agent);
        await context.SaveChangesAsync(cancellationToken);
        return agent;
    }

    public async Task AssignToUnitAsync(int unitId, int? agentId, CancellationToken cancellationToken = default)
    {
        var unit = await context.Units.SingleOrDefaultAsync(u => u.UnitId == unitId, cancellationToken)
            ?? throw new BusinessRuleException("Unit not found.");
        if (agentId is int id && !await context.Agents.AnyAsync(a => a.AgentId == id, cancellationToken))
            throw new BusinessRuleException("Agent not found.");

        unit.AgentId = agentId;
        await context.SaveChangesAsync(cancellationToken);
    }
}
