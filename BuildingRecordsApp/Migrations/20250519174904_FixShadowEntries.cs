using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuildingRecordsApp.Migrations
{
    /// <inheritdoc />
    public partial class FixShadowEntries : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agents_AgentCompanies_AgentCompanyId",
                table: "Agents");

            migrationBuilder.DropForeignKey(
                name: "FK_Leases_Units_UnitId",
                table: "Leases");

            migrationBuilder.DropForeignKey(
                name: "FK_Owners_Ownerships_OwnershipId",
                table: "Owners");

            migrationBuilder.DropForeignKey(
                name: "FK_Owners_Persons_PersonId",
                table: "Owners");

            migrationBuilder.DropForeignKey(
                name: "FK_ParkingBays_Units_UnitID",
                table: "ParkingBays");

            migrationBuilder.DropForeignKey(
                name: "FK_StoreRooms_Units_UnitId",
                table: "StoreRooms");

            migrationBuilder.DropForeignKey(
                name: "FK_TagRemoteRecords_Units_UnitId",
                table: "TagRemoteRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_Units_UnitId",
                table: "Vehicles");

            migrationBuilder.AlterColumn<int>(
                name: "UnitId",
                table: "Vehicles",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "UnitId",
                table: "TagRemoteRecords",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "UnitId",
                table: "StoreRooms",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "UnitID",
                table: "ParkingBays",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "PersonId",
                table: "Owners",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "OwnershipId",
                table: "Owners",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "UnitId",
                table: "Occupancies",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "OccupantId",
                table: "Occupancies",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "UnitId",
                table: "Leases",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "AgentCompanyId",
                table: "Agents",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_Agents_AgentCompanies_AgentCompanyId",
                table: "Agents",
                column: "AgentCompanyId",
                principalTable: "AgentCompanies",
                principalColumn: "AgentCompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Leases_Units_UnitId",
                table: "Leases",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Owners_Ownerships_OwnershipId",
                table: "Owners",
                column: "OwnershipId",
                principalTable: "Ownerships",
                principalColumn: "OwnershipId");

            migrationBuilder.AddForeignKey(
                name: "FK_Owners_Persons_PersonId",
                table: "Owners",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "PersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_ParkingBays_Units_UnitID",
                table: "ParkingBays",
                column: "UnitID",
                principalTable: "Units",
                principalColumn: "UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_StoreRooms_Units_UnitId",
                table: "StoreRooms",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_TagRemoteRecords_Units_UnitId",
                table: "TagRemoteRecords",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Units_UnitId",
                table: "Vehicles",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "UnitId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agents_AgentCompanies_AgentCompanyId",
                table: "Agents");

            migrationBuilder.DropForeignKey(
                name: "FK_Leases_Units_UnitId",
                table: "Leases");

            migrationBuilder.DropForeignKey(
                name: "FK_Owners_Ownerships_OwnershipId",
                table: "Owners");

            migrationBuilder.DropForeignKey(
                name: "FK_Owners_Persons_PersonId",
                table: "Owners");

            migrationBuilder.DropForeignKey(
                name: "FK_ParkingBays_Units_UnitID",
                table: "ParkingBays");

            migrationBuilder.DropForeignKey(
                name: "FK_StoreRooms_Units_UnitId",
                table: "StoreRooms");

            migrationBuilder.DropForeignKey(
                name: "FK_TagRemoteRecords_Units_UnitId",
                table: "TagRemoteRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_Units_UnitId",
                table: "Vehicles");

            migrationBuilder.AlterColumn<int>(
                name: "UnitId",
                table: "Vehicles",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UnitId",
                table: "TagRemoteRecords",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UnitId",
                table: "StoreRooms",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UnitID",
                table: "ParkingBays",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PersonId",
                table: "Owners",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "OwnershipId",
                table: "Owners",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UnitId",
                table: "Occupancies",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "OccupantId",
                table: "Occupancies",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UnitId",
                table: "Leases",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AgentCompanyId",
                table: "Agents",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Agents_AgentCompanies_AgentCompanyId",
                table: "Agents",
                column: "AgentCompanyId",
                principalTable: "AgentCompanies",
                principalColumn: "AgentCompanyId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Leases_Units_UnitId",
                table: "Leases",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "UnitId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Owners_Ownerships_OwnershipId",
                table: "Owners",
                column: "OwnershipId",
                principalTable: "Ownerships",
                principalColumn: "OwnershipId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Owners_Persons_PersonId",
                table: "Owners",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "PersonId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ParkingBays_Units_UnitID",
                table: "ParkingBays",
                column: "UnitID",
                principalTable: "Units",
                principalColumn: "UnitId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StoreRooms_Units_UnitId",
                table: "StoreRooms",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "UnitId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TagRemoteRecords_Units_UnitId",
                table: "TagRemoteRecords",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "UnitId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Units_UnitId",
                table: "Vehicles",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "UnitId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
