using BuildingRecordsApp.Services;
using BuildingRecordsApp.Tests.Database;

namespace BuildingRecordsApp.Tests.Services;

public class AgentServiceTests
{
    [Fact]
    public async Task Profile_uses_a_canonical_person_and_can_be_assigned_or_cleared()
    {
        await using var db = await SqliteTestDatabase.CreateAsync();
        var person = EntityFactory.Person("Agent");
        var company = EntityFactory.AgentCompany();
        var unit = EntityFactory.Unit(EntityFactory.Building(EntityFactory.Property()), "1");
        db.Context.AddRange(person, company, unit);
        await db.Context.SaveChangesAsync();
        var service = new AgentService(db.Context);

        var agent = await service.CreateProfileAsync(person.PersonId, company.AgentCompanyId);
        await service.AssignToUnitAsync(unit.UnitId, agent.AgentId);
        Assert.Equal(agent.AgentId, unit.AgentId);
        await service.AssignToUnitAsync(unit.UnitId, null);
        Assert.Null(unit.AgentId);
    }

    [Fact]
    public async Task A_person_cannot_receive_two_agent_profiles()
    {
        await using var db = await SqliteTestDatabase.CreateAsync();
        var person = EntityFactory.Person("Agent");
        var company = EntityFactory.AgentCompany();
        db.Context.AddRange(person, company);
        await db.Context.SaveChangesAsync();
        var service = new AgentService(db.Context);
        await service.CreateProfileAsync(person.PersonId, company.AgentCompanyId);

        await Assert.ThrowsAsync<BusinessRuleException>(() => service.CreateProfileAsync(person.PersonId, company.AgentCompanyId));
    }
}
