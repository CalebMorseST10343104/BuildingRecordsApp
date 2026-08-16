using BuildingRecordsApp.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace BuildingRecordsApp.Tests.Services;

public class DatabaseErrorTranslatorTests
{
    private readonly DatabaseErrorTranslator _translator = new();

    [Theory]
    [InlineData("UNIQUE constraint failed: Units.BuildingId, Units.UnitNumber", "That unit number is already in use in this building.")]
    [InlineData("UNIQUE constraint failed: Vehicles.VehicleRegistration", "That vehicle registration is already recorded.")]
    [InlineData("UNIQUE constraint failed: OwnershipContacts.OwnershipId, OwnershipContacts.PersonId", "This person is already an ownership contact for that ownership.")]
    public void Known_unique_constraint_fingerprints_receive_specific_safe_messages(string technicalMessage, string expected)
    {
        var result = _translator.Translate(UpdateException(technicalMessage, 19, 2067));

        Assert.Equal(DatabaseErrorKind.Duplicate, result.Kind);
        Assert.Equal(expected, result.UserMessage);
        Assert.DoesNotContain("constraint", result.UserMessage, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Unknown_unique_constraint_uses_a_safe_generic_conflict_message()
    {
        const string technical = "UNIQUE constraint failed: SecretTable.SecretColumn";

        var result = _translator.Translate(UpdateException(technical, 19, 2067));

        Assert.Equal(DatabaseErrorKind.Duplicate, result.Kind);
        Assert.DoesNotContain("SecretTable", result.UserMessage);
        Assert.DoesNotContain("constraint", result.UserMessage, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Foreign_key_failure_explains_that_the_record_is_still_used()
    {
        var result = _translator.Translate(UpdateException("FOREIGN KEY constraint failed", 19, 787));

        Assert.Equal(DatabaseErrorKind.RecordInUse, result.Kind);
        Assert.Contains("still used", result.UserMessage);
    }

    [Fact]
    public void Busy_database_gets_a_retry_message()
    {
        var result = _translator.Translate(new SqliteException("database is locked", 5, 5));

        Assert.Equal(DatabaseErrorKind.TemporarilyUnavailable, result.Kind);
        Assert.Contains("try again", result.UserMessage);
        Assert.DoesNotContain("locked", result.UserMessage, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Concurrency_failure_does_not_expose_framework_details()
    {
        var result = _translator.Translate(new DbUpdateConcurrencyException("Technical tracking details"));

        Assert.Equal(DatabaseErrorKind.ConcurrentChange, result.Kind);
        Assert.Contains("changed or removed", result.UserMessage);
        Assert.DoesNotContain("tracking", result.UserMessage, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Unexpected_exception_never_repeats_its_message()
    {
        const string technical = "password=definitely-not-for-the-browser";

        var result = _translator.Translate(new InvalidOperationException(technical));

        Assert.Equal(DatabaseErrorKind.Unknown, result.Kind);
        Assert.DoesNotContain(technical, result.UserMessage);
    }

    private static DbUpdateException UpdateException(string message, int errorCode, int extendedErrorCode) =>
        new("Save failed", new SqliteException(message, errorCode, extendedErrorCode));
}
