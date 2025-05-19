using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuildingRecordsApp.Migrations
{
    /// <inheritdoc />
    public partial class UnitCreationFix1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Units_Buildings_BuildingId",
                table: "Units");

            migrationBuilder.DropForeignKey(
                name: "FK_Units_Ownerships_OwnershipId",
                table: "Units");

            migrationBuilder.DropForeignKey(
                name: "FK_Units_Persons_PrimaryContactPersonId",
                table: "Units");

            migrationBuilder.AlterColumn<int>(
                name: "TagRemoteRecordId",
                table: "Units",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "PrimaryContactPersonId",
                table: "Units",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "OwnershipId",
                table: "Units",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "BuildingId",
                table: "Units",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_Units_Buildings_BuildingId",
                table: "Units",
                column: "BuildingId",
                principalTable: "Buildings",
                principalColumn: "BuildingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Units_Ownerships_OwnershipId",
                table: "Units",
                column: "OwnershipId",
                principalTable: "Ownerships",
                principalColumn: "OwnershipId");

            migrationBuilder.AddForeignKey(
                name: "FK_Units_Persons_PrimaryContactPersonId",
                table: "Units",
                column: "PrimaryContactPersonId",
                principalTable: "Persons",
                principalColumn: "PersonId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Units_Buildings_BuildingId",
                table: "Units");

            migrationBuilder.DropForeignKey(
                name: "FK_Units_Ownerships_OwnershipId",
                table: "Units");

            migrationBuilder.DropForeignKey(
                name: "FK_Units_Persons_PrimaryContactPersonId",
                table: "Units");

            migrationBuilder.AlterColumn<int>(
                name: "TagRemoteRecordId",
                table: "Units",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PrimaryContactPersonId",
                table: "Units",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "OwnershipId",
                table: "Units",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BuildingId",
                table: "Units",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Units_Buildings_BuildingId",
                table: "Units",
                column: "BuildingId",
                principalTable: "Buildings",
                principalColumn: "BuildingId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Units_Ownerships_OwnershipId",
                table: "Units",
                column: "OwnershipId",
                principalTable: "Ownerships",
                principalColumn: "OwnershipId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Units_Persons_PrimaryContactPersonId",
                table: "Units",
                column: "PrimaryContactPersonId",
                principalTable: "Persons",
                principalColumn: "PersonId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
