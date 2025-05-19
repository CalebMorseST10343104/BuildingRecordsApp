using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuildingRecordsApp.Migrations
{
    /// <inheritdoc />
    public partial class SelectListFix1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Units_Agents_AgentId",
                table: "Units");

            migrationBuilder.DropForeignKey(
                name: "FK_Units_Buildings_BuildingId",
                table: "Units");

            migrationBuilder.AddForeignKey(
                name: "FK_Units_Agents_AgentId",
                table: "Units",
                column: "AgentId",
                principalTable: "Agents",
                principalColumn: "AgentId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Units_Buildings_BuildingId",
                table: "Units",
                column: "BuildingId",
                principalTable: "Buildings",
                principalColumn: "BuildingId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Units_Agents_AgentId",
                table: "Units");

            migrationBuilder.DropForeignKey(
                name: "FK_Units_Buildings_BuildingId",
                table: "Units");

            migrationBuilder.AddForeignKey(
                name: "FK_Units_Agents_AgentId",
                table: "Units",
                column: "AgentId",
                principalTable: "Agents",
                principalColumn: "AgentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Units_Buildings_BuildingId",
                table: "Units",
                column: "BuildingId",
                principalTable: "Buildings",
                principalColumn: "BuildingId");
        }
    }
}
