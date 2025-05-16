using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuildingRecordsApp.Migrations
{
    /// <inheritdoc />
    public partial class AddOwnership : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ownership_CompanyTrusts_CompanyTrustId",
                table: "Ownership");

            migrationBuilder.DropForeignKey(
                name: "FK_Units_Ownership_OwnershipId",
                table: "Units");

            migrationBuilder.DropIndex(
                name: "IX_Units_OwnershipId",
                table: "Units");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ownership",
                table: "Ownership");

            migrationBuilder.RenameTable(
                name: "Ownership",
                newName: "Ownerships");

            migrationBuilder.RenameIndex(
                name: "IX_Ownership_CompanyTrustId",
                table: "Ownerships",
                newName: "IX_Ownerships_CompanyTrustId");

            migrationBuilder.AddColumn<string>(
                name: "UnitId",
                table: "Ownerships",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ownerships",
                table: "Ownerships",
                column: "OwnershipId");

            migrationBuilder.CreateTable(
                name: "Owners",
                columns: table => new
                {
                    OwnerId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PersonId = table.Column<int>(type: "INTEGER", nullable: false),
                    OwnershipId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Owners", x => x.OwnerId);
                    table.ForeignKey(
                        name: "FK_Owners_Ownerships_OwnershipId",
                        column: x => x.OwnershipId,
                        principalTable: "Ownerships",
                        principalColumn: "OwnershipId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Owners_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Units_OwnershipId",
                table: "Units",
                column: "OwnershipId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Owners_OwnershipId",
                table: "Owners",
                column: "OwnershipId");

            migrationBuilder.CreateIndex(
                name: "IX_Owners_PersonId",
                table: "Owners",
                column: "PersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ownerships_CompanyTrusts_CompanyTrustId",
                table: "Ownerships",
                column: "CompanyTrustId",
                principalTable: "CompanyTrusts",
                principalColumn: "CompanyTrustId");

            migrationBuilder.AddForeignKey(
                name: "FK_Units_Ownerships_OwnershipId",
                table: "Units",
                column: "OwnershipId",
                principalTable: "Ownerships",
                principalColumn: "OwnershipId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ownerships_CompanyTrusts_CompanyTrustId",
                table: "Ownerships");

            migrationBuilder.DropForeignKey(
                name: "FK_Units_Ownerships_OwnershipId",
                table: "Units");

            migrationBuilder.DropTable(
                name: "Owners");

            migrationBuilder.DropIndex(
                name: "IX_Units_OwnershipId",
                table: "Units");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ownerships",
                table: "Ownerships");

            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "Ownerships");

            migrationBuilder.RenameTable(
                name: "Ownerships",
                newName: "Ownership");

            migrationBuilder.RenameIndex(
                name: "IX_Ownerships_CompanyTrustId",
                table: "Ownership",
                newName: "IX_Ownership_CompanyTrustId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ownership",
                table: "Ownership",
                column: "OwnershipId");

            migrationBuilder.CreateIndex(
                name: "IX_Units_OwnershipId",
                table: "Units",
                column: "OwnershipId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ownership_CompanyTrusts_CompanyTrustId",
                table: "Ownership",
                column: "CompanyTrustId",
                principalTable: "CompanyTrusts",
                principalColumn: "CompanyTrustId");

            migrationBuilder.AddForeignKey(
                name: "FK_Units_Ownership_OwnershipId",
                table: "Units",
                column: "OwnershipId",
                principalTable: "Ownership",
                principalColumn: "OwnershipId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
