using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuildingRecordsApp.Migrations
{
    /// <inheritdoc />
    public partial class NormalizeCurrentRegisterModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ownerships_CompanyTrusts_CompanyTrustId",
                table: "Ownerships");

            migrationBuilder.DropIndex(
                name: "IX_Units_BuildingId",
                table: "Units");

            migrationBuilder.RenameColumn(
                name: "CompanyTrustId",
                table: "Ownerships",
                newName: "OrganizationId");

            migrationBuilder.RenameIndex(
                name: "IX_Ownerships_CompanyTrustId",
                table: "Ownerships",
                newName: "IX_Ownerships_OrganizationId");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "Owners",
                newName: "OwnershipContactId");

            migrationBuilder.RenameColumn(
                name: "PersonsOccupying",
                table: "Leases",
                newName: "DeclaredOccupantCount");

            migrationBuilder.RenameColumn(
                name: "AllowedPets",
                table: "Leases",
                newName: "PetsPresent");

            migrationBuilder.RenameColumn(
                name: "CompanyTrustId",
                table: "CompanyTrusts",
                newName: "OrganizationId");

            migrationBuilder.RenameColumn(
                name: "RegistrationNumber",
                table: "CompanyTrusts",
                newName: "RegistrationReference");

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
                name: "TagsOwner",
                table: "TagRemoteRecords",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "TagsOccupant",
                table: "TagRemoteRecords",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "TagsAgent",
                table: "TagRemoteRecords",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "RemotesOwner",
                table: "TagRemoteRecords",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "RemotesOccupant",
                table: "TagRemoteRecords",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "RemotesAgent",
                table: "TagRemoteRecords",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<int>(
                name: "PropertyId",
                table: "StoreRooms",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PropertyId",
                table: "ParkingBays",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

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

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "CompanyTrusts",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "CompanyTrusts",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrganizationType",
                table: "CompanyTrusts",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PropertyId",
                table: "Buildings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "AgentCompanyId",
                table: "Agents",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PersonId",
                table: "Agents",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Properties",
                columns: table => new
                {
                    PropertyId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Properties", x => x.PropertyId);
                });

            migrationBuilder.Sql(
                "INSERT INTO Properties (PropertyId, Name, Address) VALUES (1, 'My Property', ''); " +
                "UPDATE Buildings SET PropertyId = 1; " +
                "UPDATE ParkingBays SET PropertyId = 1; " +
                "UPDATE StoreRooms SET PropertyId = 1;");

            migrationBuilder.Sql(
                "INSERT INTO TagRemoteRecords (TagsOwner, RemotesOwner, TagsOccupant, RemotesOccupant, TagsAgent, RemotesAgent, UnitId) " +
                "SELECT 0, 0, 0, 0, 0, 0, UnitId FROM Units " +
                "WHERE NOT EXISTS (SELECT 1 FROM TagRemoteRecords WHERE TagRemoteRecords.UnitId = Units.UnitId);");

            migrationBuilder.Sql(
                "INSERT INTO Persons (FirstName, LastName, Email, PostalAddress, IdNumber, PhoneNumber) " +
                "SELECT FirstName, LastName, Email, '', '', PhoneNumber FROM Agents; " +
                "UPDATE Agents SET PersonId = (SELECT PersonId FROM Persons " +
                "WHERE Persons.FirstName = Agents.FirstName AND Persons.LastName = Agents.LastName " +
                "AND Persons.Email = Agents.Email ORDER BY PersonId DESC LIMIT 1);");

            migrationBuilder.DropColumn(name: "Email", table: "Agents");
            migrationBuilder.DropColumn(name: "FirstName", table: "Agents");
            migrationBuilder.DropColumn(name: "LastName", table: "Agents");
            migrationBuilder.DropColumn(name: "PhoneNumber", table: "Agents");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_VehicleRegistration",
                table: "Vehicles",
                column: "VehicleRegistration",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Units_BuildingId_UnitNumber",
                table: "Units",
                columns: new[] { "BuildingId", "UnitNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StoreRooms_PropertyId_StoreRoomNumber",
                table: "StoreRooms",
                columns: new[] { "PropertyId", "StoreRoomNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ParkingBays_PropertyId_ParkingBayNumber",
                table: "ParkingBays",
                columns: new[] { "PropertyId", "ParkingBayNumber" },
                unique: true);

            migrationBuilder.AddCheckConstraint(
                name: "CK_Ownership_TypeOrganization",
                table: "Ownerships",
                sql: "(OwnershipType = 'Natural' AND OrganizationId IS NULL) OR (OwnershipType = 'Juristic' AND OrganizationId IS NOT NULL)");

            migrationBuilder.CreateIndex(
                name: "IX_Buildings_PropertyId_Name",
                table: "Buildings",
                columns: new[] { "PropertyId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Agents_PersonId",
                table: "Agents",
                column: "PersonId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Properties_Name",
                table: "Properties",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Agents_Persons_PersonId",
                table: "Agents",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "PersonId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Buildings_Properties_PropertyId",
                table: "Buildings",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "PropertyId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ownerships_CompanyTrusts_OrganizationId",
                table: "Ownerships",
                column: "OrganizationId",
                principalTable: "CompanyTrusts",
                principalColumn: "OrganizationId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ParkingBays_Properties_PropertyId",
                table: "ParkingBays",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "PropertyId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StoreRooms_Properties_PropertyId",
                table: "StoreRooms",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "PropertyId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agents_Persons_PersonId",
                table: "Agents");

            migrationBuilder.DropForeignKey(
                name: "FK_Buildings_Properties_PropertyId",
                table: "Buildings");

            migrationBuilder.DropForeignKey(
                name: "FK_Ownerships_CompanyTrusts_OrganizationId",
                table: "Ownerships");

            migrationBuilder.DropForeignKey(
                name: "FK_ParkingBays_Properties_PropertyId",
                table: "ParkingBays");

            migrationBuilder.DropForeignKey(
                name: "FK_StoreRooms_Properties_PropertyId",
                table: "StoreRooms");

            migrationBuilder.DropTable(
                name: "Properties");

            migrationBuilder.DropIndex(
                name: "IX_Vehicles_VehicleRegistration",
                table: "Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_Units_BuildingId_UnitNumber",
                table: "Units");

            migrationBuilder.DropIndex(
                name: "IX_StoreRooms_PropertyId_StoreRoomNumber",
                table: "StoreRooms");

            migrationBuilder.DropIndex(
                name: "IX_ParkingBays_PropertyId_ParkingBayNumber",
                table: "ParkingBays");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Ownership_TypeOrganization",
                table: "Ownerships");

            migrationBuilder.DropIndex(
                name: "IX_Buildings_PropertyId_Name",
                table: "Buildings");

            migrationBuilder.DropIndex(
                name: "IX_Agents_PersonId",
                table: "Agents");

            migrationBuilder.DropColumn(
                name: "PropertyId",
                table: "StoreRooms");

            migrationBuilder.DropColumn(
                name: "PropertyId",
                table: "ParkingBays");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "CompanyTrusts");

            migrationBuilder.DropColumn(
                name: "OrganizationType",
                table: "CompanyTrusts");

            migrationBuilder.DropColumn(
                name: "RegistrationReference",
                table: "CompanyTrusts");

            migrationBuilder.DropColumn(
                name: "PropertyId",
                table: "Buildings");

            migrationBuilder.DropColumn(
                name: "PersonId",
                table: "Agents");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "Ownerships",
                newName: "CompanyTrustId");

            migrationBuilder.RenameIndex(
                name: "IX_Ownerships_OrganizationId",
                table: "Ownerships",
                newName: "IX_Ownerships_CompanyTrustId");

            migrationBuilder.RenameColumn(
                name: "OwnershipContactId",
                table: "Owners",
                newName: "OwnerId");

            migrationBuilder.RenameColumn(
                name: "PetsPresent",
                table: "Leases",
                newName: "PersonsOccupying");

            migrationBuilder.RenameColumn(
                name: "DeclaredOccupantCount",
                table: "Leases",
                newName: "AllowedPets");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "CompanyTrusts",
                newName: "CompanyTrustId");

            migrationBuilder.AlterColumn<int>(
                name: "UnitId",
                table: "TagRemoteRecords",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "TagsOwner",
                table: "TagRemoteRecords",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TagsOccupant",
                table: "TagRemoteRecords",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TagsAgent",
                table: "TagRemoteRecords",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RemotesOwner",
                table: "TagRemoteRecords",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RemotesOccupant",
                table: "TagRemoteRecords",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RemotesAgent",
                table: "TagRemoteRecords",
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

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "CompanyTrusts",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RegistrationNumber",
                table: "CompanyTrusts",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "AgentCompanyId",
                table: "Agents",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Agents",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Agents",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Agents",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Agents",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Units_BuildingId",
                table: "Units",
                column: "BuildingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ownerships_CompanyTrusts_CompanyTrustId",
                table: "Ownerships",
                column: "CompanyTrustId",
                principalTable: "CompanyTrusts",
                principalColumn: "CompanyTrustId",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
