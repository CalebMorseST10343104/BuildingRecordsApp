using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Services;
using BuildingRecordsApp.Tests.Database;
using Microsoft.EntityFrameworkCore;

namespace BuildingRecordsApp.Tests.Services;

public class OwnershipServiceTests
{
    [Fact]
    public async Task Ownership_can_change_between_valid_natural_and_juristic_forms()
    {
        await using var db = await SqliteTestDatabase.CreateAsync();
        var unit = EntityFactory.Unit(EntityFactory.Building(EntityFactory.Property()), "1");
        var organization = new Organization { Name = "Trust" };
        db.Context.AddRange(unit, organization);
        await db.Context.SaveChangesAsync();
        var service = new OwnershipService(db.Context);

        var ownership = await service.SetOwnershipAsync(unit.UnitId, "Natural", null);
        Assert.Null(ownership.OrganizationId);
        ownership = await service.SetOwnershipAsync(unit.UnitId, "Juristic", organization.OrganizationId);
        Assert.Equal(organization.OrganizationId, ownership.OrganizationId);
        Assert.Equal(1, await db.Context.Ownerships.CountAsync());
    }

    [Theory]
    [InlineData("Natural", true)]
    [InlineData("Juristic", false)]
    [InlineData("Unknown", false)]
    public async Task Invalid_ownership_combinations_are_rejected(string type, bool withOrganization)
    {
        await using var db = await SqliteTestDatabase.CreateAsync();
        var unit = EntityFactory.Unit(EntityFactory.Building(EntityFactory.Property()), "1");
        var organization = new Organization { Name = "Trust" };
        db.Context.AddRange(unit, organization);
        await db.Context.SaveChangesAsync();

        await Assert.ThrowsAsync<BusinessRuleException>(() => new OwnershipService(db.Context)
            .SetOwnershipAsync(unit.UnitId, type, withOrganization ? organization.OrganizationId : null));
    }

    [Fact]
    public async Task Contacts_are_idempotently_added_and_can_be_removed()
    {
        await using var db = await SqliteTestDatabase.CreateAsync();
        var unit = EntityFactory.Unit(EntityFactory.Building(EntityFactory.Property()), "1");
        var person = EntityFactory.Person("Owner");
        db.Context.AddRange(unit, person);
        await db.Context.SaveChangesAsync();
        var service = new OwnershipService(db.Context);
        var ownership = await service.SetOwnershipAsync(unit.UnitId, "Natural", null);

        await service.AddContactAsync(ownership.OwnershipId, person.PersonId);
        await service.AddContactAsync(ownership.OwnershipId, person.PersonId);
        Assert.Equal(1, await db.Context.Owners.CountAsync());
        await service.RemoveContactAsync(ownership.OwnershipId, person.PersonId);
        Assert.Empty(await db.Context.Owners.ToListAsync());
    }
}
