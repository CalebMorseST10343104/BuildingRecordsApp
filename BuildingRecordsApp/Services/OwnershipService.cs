using BuildingRecordsApp.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BuildingRecordsApp.Services;

public sealed class OwnershipService(BuildingContext context) : IOwnershipService
{
    public async Task<Ownership> SetOwnershipAsync(
        int unitId,
        string ownershipType,
        int? organizationId,
        CancellationToken cancellationToken = default)
    {
        if (!await context.Units.AnyAsync(u => u.UnitId == unitId, cancellationToken))
            throw new BusinessRuleException("Unit not found.");

        ownershipType = ownershipType.Trim();
        if (ownershipType is not ("Natural" or "Juristic"))
            throw new BusinessRuleException("Ownership type must be Natural or Juristic.");
        if (ownershipType == "Natural" && organizationId is not null)
            throw new BusinessRuleException("Natural ownership cannot have an organization.");
        if (ownershipType == "Juristic" && organizationId is null)
            throw new BusinessRuleException("Juristic ownership requires an organization.");
        if (organizationId is int id && !await context.Organizations.AnyAsync(o => o.OrganizationId == id, cancellationToken))
            throw new BusinessRuleException("Organization not found.");

        var ownership = await context.Ownerships.SingleOrDefaultAsync(o => o.UnitId == unitId, cancellationToken);
        if (ownership is null)
        {
            ownership = new Ownership { UnitId = unitId };
            context.Ownerships.Add(ownership);
        }

        ownership.OwnershipType = ownershipType;
        ownership.OrganizationId = organizationId;
        await context.SaveChangesAsync(cancellationToken);
        return ownership;
    }

    public async Task AddContactAsync(int ownershipId, int personId, CancellationToken cancellationToken = default)
    {
        if (!await context.Ownerships.AnyAsync(o => o.OwnershipId == ownershipId, cancellationToken))
            throw new BusinessRuleException("Ownership not found.");
        if (!await context.Persons.AnyAsync(p => p.PersonId == personId, cancellationToken))
            throw new BusinessRuleException("Person not found.");
        if (await context.OwnershipContacts.AnyAsync(o => o.OwnershipId == ownershipId && o.PersonId == personId, cancellationToken))
            return;

        context.OwnershipContacts.Add(new OwnershipContact { OwnershipId = ownershipId, PersonId = personId });
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveContactAsync(int ownershipId, int personId, CancellationToken cancellationToken = default)
    {
        var contact = await context.OwnershipContacts.SingleOrDefaultAsync(
            o => o.OwnershipId == ownershipId && o.PersonId == personId,
            cancellationToken);
        if (contact is null)
            return;

        context.OwnershipContacts.Remove(contact);
        await context.SaveChangesAsync(cancellationToken);
    }
}
