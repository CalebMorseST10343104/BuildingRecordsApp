using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuildingRecordsApp.Migrations
{
    /// <inheritdoc />
    public partial class UnitCreationFix5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Leases_Units_UnitNumber",
                table: "Leases");

            migrationBuilder.RenameColumn(
                name: "UnitNumber",
                table: "Leases",
                newName: "UnitId");

            migrationBuilder.RenameIndex(
                name: "IX_Leases_UnitNumber",
                table: "Leases",
                newName: "IX_Leases_UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Leases_Units_UnitId",
                table: "Leases",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "UnitId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Leases_Units_UnitId",
                table: "Leases");

            migrationBuilder.RenameColumn(
                name: "UnitId",
                table: "Leases",
                newName: "UnitNumber");

            migrationBuilder.RenameIndex(
                name: "IX_Leases_UnitId",
                table: "Leases",
                newName: "IX_Leases_UnitNumber");

            migrationBuilder.AddForeignKey(
                name: "FK_Leases_Units_UnitNumber",
                table: "Leases",
                column: "UnitNumber",
                principalTable: "Units",
                principalColumn: "UnitId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
