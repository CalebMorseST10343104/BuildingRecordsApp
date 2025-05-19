using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuildingRecordsApp.Migrations
{
    /// <inheritdoc />
    public partial class UnitCreationFix3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Units_Leases_LeaseId",
                table: "Units");

            migrationBuilder.DropIndex(
                name: "IX_Units_LeaseId",
                table: "Units");

            migrationBuilder.CreateIndex(
                name: "IX_Leases_UnitNumber",
                table: "Leases",
                column: "UnitNumber",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Leases_Units_UnitNumber",
                table: "Leases",
                column: "UnitNumber",
                principalTable: "Units",
                principalColumn: "UnitId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Leases_Units_UnitNumber",
                table: "Leases");

            migrationBuilder.DropIndex(
                name: "IX_Leases_UnitNumber",
                table: "Leases");

            migrationBuilder.CreateIndex(
                name: "IX_Units_LeaseId",
                table: "Units",
                column: "LeaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Units_Leases_LeaseId",
                table: "Units",
                column: "LeaseId",
                principalTable: "Leases",
                principalColumn: "LeaseId");
        }
    }
}
