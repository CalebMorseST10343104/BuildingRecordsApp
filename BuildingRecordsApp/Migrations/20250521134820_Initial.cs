﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuildingRecordsApp.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AgentCompanies",
                columns: table => new
                {
                    AgentCompanyId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CompanyName = table.Column<string>(type: "TEXT", nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: false),
                    RegistrationNumber = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgentCompanies", x => x.AgentCompanyId);
                });

            migrationBuilder.CreateTable(
                name: "Buildings",
                columns: table => new
                {
                    BuildingId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: false),
                    NumberOfUnits = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberOfFloors = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buildings", x => x.BuildingId);
                });

            migrationBuilder.CreateTable(
                name: "CompanyTrusts",
                columns: table => new
                {
                    CompanyTrustId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: false),
                    RegistrationNumber = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyTrusts", x => x.CompanyTrustId);
                });

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    PersonId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Surname = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    PostalAddress = table.Column<string>(type: "TEXT", nullable: false),
                    IdNumber = table.Column<string>(type: "TEXT", nullable: false),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.PersonId);
                });

            migrationBuilder.CreateTable(
                name: "Agents",
                columns: table => new
                {
                    AgentId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: false),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    AgentCompanyId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agents", x => x.AgentId);
                    table.ForeignKey(
                        name: "FK_Agents_AgentCompanies_AgentCompanyId",
                        column: x => x.AgentCompanyId,
                        principalTable: "AgentCompanies",
                        principalColumn: "AgentCompanyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Units",
                columns: table => new
                {
                    UnitId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UnitNumber = table.Column<string>(type: "TEXT", nullable: false),
                    Bedrooms = table.Column<int>(type: "INTEGER", nullable: false),
                    DbInverter = table.Column<bool>(type: "INTEGER", nullable: false),
                    Housekeeping = table.Column<bool>(type: "INTEGER", nullable: false),
                    PetFriendly = table.Column<bool>(type: "INTEGER", nullable: false),
                    SublettingAllowed = table.Column<bool>(type: "INTEGER", nullable: false),
                    AirconditioningUnits = table.Column<int>(type: "INTEGER", nullable: false),
                    BuildingId = table.Column<int>(type: "INTEGER", nullable: true),
                    PrimaryContactPersonId = table.Column<int>(type: "INTEGER", nullable: true),
                    OwnershipId = table.Column<int>(type: "INTEGER", nullable: true),
                    AgentId = table.Column<int>(type: "INTEGER", nullable: true),
                    LeaseId = table.Column<int>(type: "INTEGER", nullable: true),
                    TagRemoteRecordId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Units", x => x.UnitId);
                    table.ForeignKey(
                        name: "FK_Units_Agents_AgentId",
                        column: x => x.AgentId,
                        principalTable: "Agents",
                        principalColumn: "AgentId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Units_Buildings_BuildingId",
                        column: x => x.BuildingId,
                        principalTable: "Buildings",
                        principalColumn: "BuildingId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Units_Persons_PrimaryContactPersonId",
                        column: x => x.PrimaryContactPersonId,
                        principalTable: "Persons",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Leases",
                columns: table => new
                {
                    LeaseId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LeaseHolderName = table.Column<string>(type: "TEXT", nullable: false),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PersonsOccupying = table.Column<int>(type: "INTEGER", nullable: false),
                    SignedRules = table.Column<bool>(type: "INTEGER", nullable: false),
                    AllowedPets = table.Column<bool>(type: "INTEGER", nullable: false),
                    EmergencyContactNumber = table.Column<string>(type: "TEXT", nullable: false),
                    UnitId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leases", x => x.LeaseId);
                    table.ForeignKey(
                        name: "FK_Leases_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "UnitId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Occupancies",
                columns: table => new
                {
                    UnitId = table.Column<int>(type: "INTEGER", nullable: false),
                    OccupantId = table.Column<int>(type: "INTEGER", nullable: false),
                    OccupancyId = table.Column<int>(type: "INTEGER", nullable: false),
                    OccupationType = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Occupancies", x => new { x.UnitId, x.OccupantId });
                    table.ForeignKey(
                        name: "FK_Occupancies_Persons_OccupantId",
                        column: x => x.OccupantId,
                        principalTable: "Persons",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Occupancies_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "UnitId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ownerships",
                columns: table => new
                {
                    OwnershipId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OwnershipType = table.Column<string>(type: "TEXT", nullable: false),
                    UnitId = table.Column<int>(type: "INTEGER", nullable: true),
                    CompanyTrustId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ownerships", x => x.OwnershipId);
                    table.ForeignKey(
                        name: "FK_Ownerships_CompanyTrusts_CompanyTrustId",
                        column: x => x.CompanyTrustId,
                        principalTable: "CompanyTrusts",
                        principalColumn: "CompanyTrustId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ownerships_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "UnitId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ParkingBays",
                columns: table => new
                {
                    ParkingBayID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ParkingBayNumber = table.Column<string>(type: "TEXT", nullable: false),
                    UnitID = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParkingBays", x => x.ParkingBayID);
                    table.ForeignKey(
                        name: "FK_ParkingBays_Units_UnitID",
                        column: x => x.UnitID,
                        principalTable: "Units",
                        principalColumn: "UnitId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "StoreRooms",
                columns: table => new
                {
                    StoreRoomId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StoreRoomNumber = table.Column<string>(type: "TEXT", nullable: false),
                    UnitId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreRooms", x => x.StoreRoomId);
                    table.ForeignKey(
                        name: "FK_StoreRooms_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "UnitId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "TagRemoteRecords",
                columns: table => new
                {
                    TagRemoteRecordId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TagsOwner = table.Column<int>(type: "INTEGER", nullable: false),
                    RemotesOwner = table.Column<int>(type: "INTEGER", nullable: false),
                    TagsOccupant = table.Column<int>(type: "INTEGER", nullable: false),
                    RemotesOccupant = table.Column<int>(type: "INTEGER", nullable: false),
                    TagsAgent = table.Column<int>(type: "INTEGER", nullable: false),
                    RemotesAgent = table.Column<int>(type: "INTEGER", nullable: false),
                    UnitId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagRemoteRecords", x => x.TagRemoteRecordId);
                    table.ForeignKey(
                        name: "FK_TagRemoteRecords_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "UnitId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    VehicleId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    VehicleRegistration = table.Column<string>(type: "TEXT", nullable: false),
                    VehicleModel = table.Column<string>(type: "TEXT", nullable: false),
                    VehicleMake = table.Column<string>(type: "TEXT", nullable: false),
                    VehicleColor = table.Column<string>(type: "TEXT", nullable: false),
                    UnitId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.VehicleId);
                    table.ForeignKey(
                        name: "FK_Vehicles_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "UnitId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Owners",
                columns: table => new
                {
                    PersonId = table.Column<int>(type: "INTEGER", nullable: false),
                    OwnershipId = table.Column<int>(type: "INTEGER", nullable: false),
                    OwnerId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Owners", x => new { x.OwnershipId, x.PersonId });
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
                name: "IX_Agents_AgentCompanyId",
                table: "Agents",
                column: "AgentCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Leases_UnitId",
                table: "Leases",
                column: "UnitId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Occupancies_OccupantId",
                table: "Occupancies",
                column: "OccupantId");

            migrationBuilder.CreateIndex(
                name: "IX_Owners_PersonId",
                table: "Owners",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Ownerships_CompanyTrustId",
                table: "Ownerships",
                column: "CompanyTrustId");

            migrationBuilder.CreateIndex(
                name: "IX_Ownerships_UnitId",
                table: "Ownerships",
                column: "UnitId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ParkingBays_UnitID",
                table: "ParkingBays",
                column: "UnitID");

            migrationBuilder.CreateIndex(
                name: "IX_StoreRooms_UnitId",
                table: "StoreRooms",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_TagRemoteRecords_UnitId",
                table: "TagRemoteRecords",
                column: "UnitId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Units_AgentId",
                table: "Units",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_Units_BuildingId",
                table: "Units",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_Units_PrimaryContactPersonId",
                table: "Units",
                column: "PrimaryContactPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_UnitId",
                table: "Vehicles",
                column: "UnitId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Leases");

            migrationBuilder.DropTable(
                name: "Occupancies");

            migrationBuilder.DropTable(
                name: "Owners");

            migrationBuilder.DropTable(
                name: "ParkingBays");

            migrationBuilder.DropTable(
                name: "StoreRooms");

            migrationBuilder.DropTable(
                name: "TagRemoteRecords");

            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.DropTable(
                name: "Ownerships");

            migrationBuilder.DropTable(
                name: "CompanyTrusts");

            migrationBuilder.DropTable(
                name: "Units");

            migrationBuilder.DropTable(
                name: "Agents");

            migrationBuilder.DropTable(
                name: "Buildings");

            migrationBuilder.DropTable(
                name: "Persons");

            migrationBuilder.DropTable(
                name: "AgentCompanies");
        }
    }
}
