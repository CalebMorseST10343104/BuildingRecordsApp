using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuildingRecordsApp.Migrations
{
    /// <inheritdoc />
    public partial class ContextUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ownerships_CompanyTrusts_CompanyTrustId",
                table: "Ownerships");

            migrationBuilder.AddForeignKey(
                name: "FK_Ownerships_CompanyTrusts_CompanyTrustId",
                table: "Ownerships",
                column: "CompanyTrustId",
                principalTable: "CompanyTrusts",
                principalColumn: "CompanyTrustId",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ownerships_CompanyTrusts_CompanyTrustId",
                table: "Ownerships");

            migrationBuilder.AddForeignKey(
                name: "FK_Ownerships_CompanyTrusts_CompanyTrustId",
                table: "Ownerships",
                column: "CompanyTrustId",
                principalTable: "CompanyTrusts",
                principalColumn: "CompanyTrustId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
