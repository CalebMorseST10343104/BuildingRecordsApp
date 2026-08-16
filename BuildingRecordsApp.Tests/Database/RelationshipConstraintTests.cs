using BuildingRecordsApp.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BuildingRecordsApp.Tests.Database;

public class RelationshipConstraintTests
{
    [Fact]
    public async Task Person_can_occupy_multiple_units_but_not_duplicate_the_same_unit()
    {
        await using var db = await SqliteTestDatabase.CreateAsync();
        var building = EntityFactory.Building(EntityFactory.Property());
        var person = EntityFactory.Person("Alex");
        var first = EntityFactory.Unit(building, "1");
        var second = EntityFactory.Unit(building, "2");
        db.Context.Occupancies.AddRange(
            new() { Occupant = person, Unit = first },
            new() { Occupant = person, Unit = second });
        await db.Context.SaveChangesAsync();
        db.Context.Occupancies.Add(new() { OccupantId = person.PersonId, UnitId = first.UnitId });
        await Assert.ThrowsAsync<DbUpdateException>(() => db.Context.SaveChangesAsync());
    }

    [Fact]
    public async Task Ownership_can_have_multiple_contacts_but_not_duplicate_a_person()
    {
        await using var db = await SqliteTestDatabase.CreateAsync();
        var unit = EntityFactory.Unit(EntityFactory.Building(EntityFactory.Property()), "1");
        var ownership = new Ownership { Unit = unit, OwnershipType = "Natural" };
        var first = EntityFactory.Person("Alex");
        var second = EntityFactory.Person("Sam");
        db.Context.OwnershipContacts.AddRange(
            new() { Ownership = ownership, Person = first },
            new() { Ownership = ownership, Person = second });
        await db.Context.SaveChangesAsync();
        db.Context.OwnershipContacts.Add(new() { OwnershipId = ownership.OwnershipId, PersonId = first.PersonId });
        await Assert.ThrowsAsync<DbUpdateException>(() => db.Context.SaveChangesAsync());
    }

    [Fact]
    public async Task Unit_can_have_only_one_lease_ownership_and_access_record()
    {
        await using var db = await SqliteTestDatabase.CreateAsync();
        var unit = EntityFactory.Unit(EntityFactory.Building(EntityFactory.Property()), "1");
        db.Context.Add(unit);
        await db.Context.SaveChangesAsync();
        db.Context.ChangeTracker.Clear();

        db.Context.Leases.AddRange(new() { UnitId = unit.UnitId }, new() { UnitId = unit.UnitId });
        await Assert.ThrowsAsync<DbUpdateException>(() => db.Context.SaveChangesAsync());
        db.Context.ChangeTracker.Clear();

        db.Context.Ownerships.AddRange(new() { UnitId = unit.UnitId, OwnershipType = "Natural" }, new() { UnitId = unit.UnitId, OwnershipType = "Natural" });
        await Assert.ThrowsAsync<DbUpdateException>(() => db.Context.SaveChangesAsync());
        db.Context.ChangeTracker.Clear();

        db.Context.AccessDeviceCounts.AddRange(new() { UnitId = unit.UnitId }, new() { UnitId = unit.UnitId });
        await Assert.ThrowsAsync<DbUpdateException>(() => db.Context.SaveChangesAsync());
    }

    [Fact]
    public async Task Person_can_have_only_one_agent_profile()
    {
        await using var db = await SqliteTestDatabase.CreateAsync();
        var person = EntityFactory.Person("Agent");
        var company = EntityFactory.AgentCompany();
        db.Context.AddRange(person, company);
        await db.Context.SaveChangesAsync();
        db.Context.ChangeTracker.Clear();
        db.Context.Agents.AddRange(
            new() { PersonId = person.PersonId, AgentCompanyId = company.AgentCompanyId },
            new() { PersonId = person.PersonId, AgentCompanyId = company.AgentCompanyId });
        await Assert.ThrowsAsync<DbUpdateException>(() => db.Context.SaveChangesAsync());
    }
}
