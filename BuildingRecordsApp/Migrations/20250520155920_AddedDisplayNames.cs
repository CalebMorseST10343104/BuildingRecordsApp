using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuildingRecordsApp.Migrations
{
    /// <inheritdoc />
    public partial class AddedDisplayNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OwnershipId",
                table: "Persons",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Persons_OwnershipId",
                table: "Persons",
                column: "OwnershipId");

            migrationBuilder.AddForeignKey(
                name: "FK_Persons_Ownerships_OwnershipId",
                table: "Persons",
                column: "OwnershipId",
                principalTable: "Ownerships",
                principalColumn: "OwnershipId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Persons_Ownerships_OwnershipId",
                table: "Persons");

            migrationBuilder.DropIndex(
                name: "IX_Persons_OwnershipId",
                table: "Persons");

            migrationBuilder.DropColumn(
                name: "OwnershipId",
                table: "Persons");
        }
    }
}
