using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace BuildingRecordsApp.Services;

public sealed class DatabaseErrorTranslator : IDatabaseErrorTranslator
{
    private const string GenericConflict = "That change conflicts with information already in the register. Please review the record and try again.";

    private static readonly IReadOnlyList<(string Fingerprint, string Message)> DuplicateFingerprints =
    [
        ("Properties.Name", "A property with that name is already recorded."),
        ("Buildings.PropertyId, Buildings.Name", "That building name is already in use in this property."),
        ("Units.BuildingId, Units.UnitNumber", "That unit number is already in use in this building."),
        ("ParkingBays.PropertyId, ParkingBays.ParkingBayNumber", "That parking bay number is already in use in this property."),
        ("StoreRooms.PropertyId, StoreRooms.StoreRoomNumber", "That storeroom number is already in use in this property."),
        ("Vehicles.VehicleRegistration", "That vehicle registration is already recorded."),
        ("Occupancies.UnitId, Occupancies.OccupantId", "This person is already recorded as an occupant of that unit."),
        ("OwnershipContacts.OwnershipId, OwnershipContacts.PersonId", "This person is already an ownership contact for that ownership."),
        ("Ownerships.UnitId", "That unit already has an ownership record."),
        ("Leases.UnitId", "That unit already has a lease summary."),
        ("AccessDeviceCounts.UnitId", "That unit already has a tag and remote count record."),
        ("Agents.PersonId", "That person already has a letting-agent profile.")
    ];

    public DatabaseErrorMessage Translate(Exception exception)
    {
        if (exception is DbUpdateConcurrencyException)
            return new(DatabaseErrorKind.ConcurrentChange, "This record was changed or removed after you opened it. Return to the list and try again.");

        var sqlite = FindSqliteException(exception);
        if (sqlite is null)
            return new(DatabaseErrorKind.Unknown, "We couldn't save that change. No information was lost; please try again.");

        if (sqlite.SqliteErrorCode is 5 or 6)
            return new(DatabaseErrorKind.TemporarilyUnavailable, "The register is busy with another change. Please wait a moment and try again.");

        if (sqlite.SqliteErrorCode is 8 or 10 or 11 or 13 or 14 or 26)
            return new(DatabaseErrorKind.TemporarilyUnavailable, "The register database is temporarily unavailable. Please close and reopen the application, then try again.");

        if (sqlite.SqliteErrorCode != 19)
            return new(DatabaseErrorKind.Unknown, "We couldn't save that change. No information was lost; please try again.");

        return sqlite.SqliteExtendedErrorCode switch
        {
            2067 or 1555 => DuplicateMessage(sqlite.Message),
            787 => new(DatabaseErrorKind.RecordInUse, "That change cannot be made because the record is still used elsewhere in the register."),
            1299 => new(DatabaseErrorKind.MissingRequiredValue, "A required value is missing. Please review the record and try again."),
            275 => new(DatabaseErrorKind.InvalidValue, "One of the supplied values is not valid. Please review the record and try again."),
            _ => new(DatabaseErrorKind.Unknown, GenericConflict)
        };
    }

    private static DatabaseErrorMessage DuplicateMessage(string technicalMessage)
    {
        var match = DuplicateFingerprints.FirstOrDefault(item =>
            technicalMessage.Contains(item.Fingerprint, StringComparison.OrdinalIgnoreCase));

        return new(
            DatabaseErrorKind.Duplicate,
            match == default ? GenericConflict : match.Message);
    }

    private static SqliteException? FindSqliteException(Exception exception)
    {
        for (Exception? current = exception; current is not null; current = current.InnerException)
        {
            if (current is SqliteException sqlite)
                return sqlite;
        }

        return null;
    }
}
