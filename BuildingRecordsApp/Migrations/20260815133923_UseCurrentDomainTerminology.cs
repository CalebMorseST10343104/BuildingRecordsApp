using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuildingRecordsApp.Migrations
{
    /// <inheritdoc />
    public partial class UseCurrentDomainTerminology : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Owners_Ownerships_OwnershipId",
                table: "Owners");

            migrationBuilder.DropForeignKey(
                name: "FK_Owners_Persons_PersonId",
                table: "Owners");

            migrationBuilder.DropForeignKey(
                name: "FK_Ownerships_CompanyTrusts_OrganizationId",
                table: "Ownerships");

            migrationBuilder.RenameTable(
                name: "TagRemoteRecords",
                newName: "AccessDeviceCounts");

            migrationBuilder.RenameColumn(name: "TagRemoteRecordId", table: "AccessDeviceCounts", newName: "AccessDeviceCountId");
            migrationBuilder.RenameColumn(name: "TagsOwner", table: "AccessDeviceCounts", newName: "OwnershipContactTagCount");
            migrationBuilder.RenameColumn(name: "RemotesOwner", table: "AccessDeviceCounts", newName: "OwnershipContactRemoteCount");
            migrationBuilder.RenameColumn(name: "TagsOccupant", table: "AccessDeviceCounts", newName: "OccupantTagCount");
            migrationBuilder.RenameColumn(name: "RemotesOccupant", table: "AccessDeviceCounts", newName: "OccupantRemoteCount");
            migrationBuilder.RenameColumn(name: "TagsAgent", table: "AccessDeviceCounts", newName: "AgentTagCount");
            migrationBuilder.RenameColumn(name: "RemotesAgent", table: "AccessDeviceCounts", newName: "AgentRemoteCount");
            migrationBuilder.RenameIndex(name: "IX_TagRemoteRecords_UnitId", table: "AccessDeviceCounts", newName: "IX_AccessDeviceCounts_UnitId");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Owners",
                table: "Owners");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CompanyTrusts",
                table: "CompanyTrusts");

            migrationBuilder.RenameTable(
                name: "Owners",
                newName: "OwnershipContacts");

            migrationBuilder.RenameTable(
                name: "CompanyTrusts",
                newName: "Organizations");

            migrationBuilder.RenameColumn(
                name: "TagRemoteRecordId",
                table: "Units",
                newName: "AccessDeviceCountId");

            migrationBuilder.RenameIndex(
                name: "IX_Owners_PersonId",
                table: "OwnershipContacts",
                newName: "IX_OwnershipContacts_PersonId");

            migrationBuilder.RenameIndex(
                name: "IX_Owners_OwnershipId_PersonId",
                table: "OwnershipContacts",
                newName: "IX_OwnershipContacts_OwnershipId_PersonId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OwnershipContacts",
                table: "OwnershipContacts",
                column: "OwnershipContactId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Organizations",
                table: "Organizations",
                column: "OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_OwnershipContacts_Ownerships_OwnershipId",
                table: "OwnershipContacts",
                column: "OwnershipId",
                principalTable: "Ownerships",
                principalColumn: "OwnershipId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OwnershipContacts_Persons_PersonId",
                table: "OwnershipContacts",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "PersonId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ownerships_Organizations_OrganizationId",
                table: "Ownerships",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "OrganizationId",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OwnershipContacts_Ownerships_OwnershipId",
                table: "OwnershipContacts");

            migrationBuilder.DropForeignKey(
                name: "FK_OwnershipContacts_Persons_PersonId",
                table: "OwnershipContacts");

            migrationBuilder.DropForeignKey(
                name: "FK_Ownerships_Organizations_OrganizationId",
                table: "Ownerships");

            migrationBuilder.RenameIndex(name: "IX_AccessDeviceCounts_UnitId", table: "AccessDeviceCounts", newName: "IX_TagRemoteRecords_UnitId");
            migrationBuilder.RenameColumn(name: "AccessDeviceCountId", table: "AccessDeviceCounts", newName: "TagRemoteRecordId");
            migrationBuilder.RenameColumn(name: "OwnershipContactTagCount", table: "AccessDeviceCounts", newName: "TagsOwner");
            migrationBuilder.RenameColumn(name: "OwnershipContactRemoteCount", table: "AccessDeviceCounts", newName: "RemotesOwner");
            migrationBuilder.RenameColumn(name: "OccupantTagCount", table: "AccessDeviceCounts", newName: "TagsOccupant");
            migrationBuilder.RenameColumn(name: "OccupantRemoteCount", table: "AccessDeviceCounts", newName: "RemotesOccupant");
            migrationBuilder.RenameColumn(name: "AgentTagCount", table: "AccessDeviceCounts", newName: "TagsAgent");
            migrationBuilder.RenameColumn(name: "AgentRemoteCount", table: "AccessDeviceCounts", newName: "RemotesAgent");
            migrationBuilder.RenameTable(name: "AccessDeviceCounts", newName: "TagRemoteRecords");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OwnershipContacts",
                table: "OwnershipContacts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Organizations",
                table: "Organizations");

            migrationBuilder.RenameTable(
                name: "OwnershipContacts",
                newName: "Owners");

            migrationBuilder.RenameTable(
                name: "Organizations",
                newName: "CompanyTrusts");

            migrationBuilder.RenameColumn(
                name: "AccessDeviceCountId",
                table: "Units",
                newName: "TagRemoteRecordId");

            migrationBuilder.RenameIndex(
                name: "IX_OwnershipContacts_PersonId",
                table: "Owners",
                newName: "IX_Owners_PersonId");

            migrationBuilder.RenameIndex(
                name: "IX_OwnershipContacts_OwnershipId_PersonId",
                table: "Owners",
                newName: "IX_Owners_OwnershipId_PersonId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Owners",
                table: "Owners",
                column: "OwnershipContactId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CompanyTrusts",
                table: "CompanyTrusts",
                column: "OrganizationId");

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
                name: "FK_Ownerships_CompanyTrusts_OrganizationId",
                table: "Ownerships",
                column: "OrganizationId",
                principalTable: "CompanyTrusts",
                principalColumn: "OrganizationId",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
