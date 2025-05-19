using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuildingRecordsApp.Migrations
{
    /// <inheritdoc />
    public partial class OverflowFixAttempt1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Units_LeaseId",
                table: "Units");

            migrationBuilder.CreateIndex(
                name: "IX_Units_LeaseId",
                table: "Units",
                column: "LeaseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Units_LeaseId",
                table: "Units");

            migrationBuilder.CreateIndex(
                name: "IX_Units_LeaseId",
                table: "Units",
                column: "LeaseId",
                unique: true);
        }
    }
}
