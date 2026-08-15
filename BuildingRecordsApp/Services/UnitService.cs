using BuildingRecordsApp.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BuildingRecordsApp.Services;

public sealed class UnitService(BuildingContext context) : IUnitService
{
    public async Task<Unit> CreateAsync(Unit unit, CancellationToken cancellationToken = default)
    {
        if (unit.BuildingId == 0 && unit.Building is null)
            throw new BusinessRuleException("A unit must belong to a building.");

        if (string.IsNullOrWhiteSpace(unit.UnitNumber))
            throw new BusinessRuleException("A unit number is required.");

        unit.UnitNumber = unit.UnitNumber.Trim();
        unit.TagRemoteRecord ??= new TagRemoteRecord();

        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        context.Units.Add(unit);
        await context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return unit;
    }
}
