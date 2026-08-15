using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BuildingRecordsApp.Tests.Database;

public class MigrationTests
{
    [Fact]
    public async Task Latest_migration_preserves_valid_rows_and_removes_parentless_rows()
    {
        var path = Path.GetTempFileName();
        try
        {
            var options = new DbContextOptionsBuilder<BuildingContext>()
                .UseSqlite($"Data Source={path};Foreign Keys=True").Options;
            await using var context = new BuildingContext(options);
            var migrator = context.GetService<IMigrator>();
            await migrator.MigrateAsync("20260815121630_NormalizeCurrentRegisterModel");

            await context.Database.ExecuteSqlRawAsync("""
                INSERT INTO Buildings (Name, Address, NumberOfUnits, NumberOfFloors, PropertyId)
                    VALUES ('A', '', 1, 1, 1);
                INSERT INTO Units (UnitNumber, Bedrooms, DbInverter, Housekeeping, PetFriendly,
                    SublettingAllowed, AirconditioningUnits, BuildingId)
                    VALUES ('101', 1, 0, 0, 1, 0, 0, last_insert_rowid());
                INSERT INTO Vehicles (VehicleRegistration, VehicleModel, VehicleMake, VehicleColor, UnitId)
                    VALUES ('VALID', '', '', '', last_insert_rowid());
                INSERT INTO Vehicles (VehicleRegistration, VehicleModel, VehicleMake, VehicleColor, UnitId)
                    VALUES ('ORPHAN', '', '', '', NULL);
                """);

            await migrator.MigrateAsync();

            Assert.Equal(1, await context.Vehicles.CountAsync());
            Assert.Equal("VALID", await context.Vehicles.Select(v => v.VehicleRegistration).SingleAsync());
            var violations = await ReadForeignKeyViolationsAsync(context);
            Assert.Equal(0, violations);
        }
        finally
        {
            File.Delete(path);
        }
    }

    private static async Task<long> ReadForeignKeyViolationsAsync(BuildingContext context)
    {
        var connection = (SqliteConnection)context.Database.GetDbConnection();
        await connection.OpenAsync();
        await using var command = connection.CreateCommand();
        command.CommandText = "SELECT COUNT(*) FROM pragma_foreign_key_check";
        return (long)(await command.ExecuteScalarAsync() ?? 0L);
    }
}
