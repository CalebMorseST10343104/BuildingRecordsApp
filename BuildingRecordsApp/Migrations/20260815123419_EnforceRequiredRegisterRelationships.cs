using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuildingRecordsApp.Migrations
{
    /// <inheritdoc />
    public partial class EnforceRequiredRegisterRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "UPDATE Units SET BuildingId = (SELECT BuildingId FROM Buildings ORDER BY BuildingId LIMIT 1) " +
                "WHERE BuildingId IS NULL; " +
                "DELETE FROM Vehicles WHERE UnitId IS NULL; " +
                "DELETE FROM Ownerships WHERE UnitId IS NULL; " +
                "DELETE FROM Occupancies WHERE UnitId IS NULL OR OccupantId IS NULL; " +
                "DELETE FROM Leases WHERE UnitId IS NULL;");

            migrationBuilder.AlterColumn<int>(
                name: "UnitId",
                table: "Vehicles",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BuildingId",
                table: "Units",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UnitId",
                table: "Ownerships",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UnitId",
                table: "Occupancies",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "OccupantId",
                table: "Occupancies",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UnitId",
                table: "Leases",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddCheckConstraint(
                name: "CK_TagRemoteRecord_NonnegativeCounts",
                table: "TagRemoteRecords",
                sql: "(TagsOwner IS NULL OR TagsOwner >= 0) AND (RemotesOwner IS NULL OR RemotesOwner >= 0) AND (TagsOccupant IS NULL OR TagsOccupant >= 0) AND (RemotesOccupant IS NULL OR RemotesOccupant >= 0) AND (TagsAgent IS NULL OR TagsAgent >= 0) AND (RemotesAgent IS NULL OR RemotesAgent >= 0)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_TagRemoteRecord_NonnegativeCounts",
                table: "TagRemoteRecords");

            migrationBuilder.AlterColumn<int>(
                name: "UnitId",
                table: "Vehicles",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "BuildingId",
                table: "Units",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "UnitId",
                table: "Ownerships",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "UnitId",
                table: "Occupancies",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "OccupantId",
                table: "Occupancies",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "UnitId",
                table: "Leases",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");
        }
    }
}
