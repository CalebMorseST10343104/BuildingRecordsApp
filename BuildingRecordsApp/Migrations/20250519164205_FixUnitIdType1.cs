using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuildingRecordsApp.Migrations
{
    /// <inheritdoc />
    public partial class FixUnitIdType1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Units_Ownerships_OwnershipId",
                table: "Units");

            migrationBuilder.DropIndex(
                name: "IX_Units_OwnershipId",
                table: "Units");

            migrationBuilder.AlterColumn<int>(
                name: "UnitId",
                table: "Ownerships",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.CreateIndex(
                name: "IX_Ownerships_UnitId",
                table: "Ownerships",
                column: "UnitId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Ownerships_Units_UnitId",
                table: "Ownerships",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "UnitId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ownerships_Units_UnitId",
                table: "Ownerships");

            migrationBuilder.DropIndex(
                name: "IX_Ownerships_UnitId",
                table: "Ownerships");

            migrationBuilder.AlterColumn<string>(
                name: "UnitId",
                table: "Ownerships",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Units_OwnershipId",
                table: "Units",
                column: "OwnershipId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Units_Ownerships_OwnershipId",
                table: "Units",
                column: "OwnershipId",
                principalTable: "Ownerships",
                principalColumn: "OwnershipId");
        }
    }
}
