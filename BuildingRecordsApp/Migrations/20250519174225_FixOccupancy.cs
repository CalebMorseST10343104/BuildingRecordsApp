using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuildingRecordsApp.Migrations
{
    /// <inheritdoc />
    public partial class FixOccupancy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Occupancies_Persons_OccupantId",
                table: "Occupancies");

            migrationBuilder.DropForeignKey(
                name: "FK_Occupancies_Units_UnitId",
                table: "Occupancies");

            migrationBuilder.AddForeignKey(
                name: "FK_Occupancies_Persons_OccupantId",
                table: "Occupancies",
                column: "OccupantId",
                principalTable: "Persons",
                principalColumn: "PersonId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Occupancies_Units_UnitId",
                table: "Occupancies",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "UnitId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Occupancies_Persons_OccupantId",
                table: "Occupancies");

            migrationBuilder.DropForeignKey(
                name: "FK_Occupancies_Units_UnitId",
                table: "Occupancies");

            migrationBuilder.AddForeignKey(
                name: "FK_Occupancies_Persons_OccupantId",
                table: "Occupancies",
                column: "OccupantId",
                principalTable: "Persons",
                principalColumn: "PersonId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Occupancies_Units_UnitId",
                table: "Occupancies",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "UnitId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
