using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuildingRecordsApp.Migrations
{
    /// <inheritdoc />
    public partial class UseSurrogateKeysForOwnerAndOccupancy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Existing join records used composite keys, so their unused surrogate IDs are all zero.
            // Populate stable, unique values before rebuilding the SQLite tables around those IDs.
            migrationBuilder.Sql("UPDATE Owners SET OwnerId = rowid WHERE OwnerId = 0;");
            migrationBuilder.Sql("UPDATE Occupancies SET OccupancyId = rowid WHERE OccupancyId = 0;");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Owners",
                table: "Owners");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Occupancies",
                table: "Occupancies");

            migrationBuilder.AlterColumn<int>(
                name: "OwnerId",
                table: "Owners",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<int>(
                name: "PersonId",
                table: "Owners",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "OwnershipId",
                table: "Owners",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "OccupancyId",
                table: "Occupancies",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<int>(
                name: "OccupantId",
                table: "Occupancies",
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

            migrationBuilder.AddPrimaryKey(
                name: "PK_Owners",
                table: "Owners",
                column: "OwnerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Occupancies",
                table: "Occupancies",
                column: "OccupancyId");

            migrationBuilder.CreateIndex(
                name: "IX_Owners_OwnershipId_PersonId",
                table: "Owners",
                columns: new[] { "OwnershipId", "PersonId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Occupancies_UnitId_OccupantId",
                table: "Occupancies",
                columns: new[] { "UnitId", "OccupantId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Owners",
                table: "Owners");

            migrationBuilder.DropIndex(
                name: "IX_Owners_OwnershipId_PersonId",
                table: "Owners");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Occupancies",
                table: "Occupancies");

            migrationBuilder.DropIndex(
                name: "IX_Occupancies_UnitId_OccupantId",
                table: "Occupancies");

            migrationBuilder.AlterColumn<int>(
                name: "PersonId",
                table: "Owners",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "OwnershipId",
                table: "Owners",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "OwnerId",
                table: "Owners",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

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
                name: "OccupancyId",
                table: "Occupancies",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Owners",
                table: "Owners",
                columns: new[] { "OwnershipId", "PersonId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Occupancies",
                table: "Occupancies",
                columns: new[] { "UnitId", "OccupantId" });
        }
    }
}
