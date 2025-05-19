using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuildingRecordsApp.Migrations
{
    /// <inheritdoc />
    public partial class WorkingBuild16052025 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Persons_Occupancies_OccupancyId",
                table: "Persons");

            migrationBuilder.DropForeignKey(
                name: "FK_Units_TagRemoteRecords_TagRemoteRecordId",
                table: "Units");

            migrationBuilder.DropIndex(
                name: "IX_Units_TagRemoteRecordId",
                table: "Units");

            migrationBuilder.DropIndex(
                name: "IX_Persons_OccupancyId",
                table: "Persons");

            migrationBuilder.DropColumn(
                name: "OccupancyId",
                table: "Persons");

            migrationBuilder.AddColumn<int>(
                name: "UnitId",
                table: "TagRemoteRecords",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OccupantId",
                table: "Occupancies",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TagRemoteRecords_UnitId",
                table: "TagRemoteRecords",
                column: "UnitId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Occupancies_OccupantId",
                table: "Occupancies",
                column: "OccupantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Occupancies_Persons_OccupantId",
                table: "Occupancies",
                column: "OccupantId",
                principalTable: "Persons",
                principalColumn: "PersonId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TagRemoteRecords_Units_UnitId",
                table: "TagRemoteRecords",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "UnitId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Occupancies_Persons_OccupantId",
                table: "Occupancies");

            migrationBuilder.DropForeignKey(
                name: "FK_TagRemoteRecords_Units_UnitId",
                table: "TagRemoteRecords");

            migrationBuilder.DropIndex(
                name: "IX_TagRemoteRecords_UnitId",
                table: "TagRemoteRecords");

            migrationBuilder.DropIndex(
                name: "IX_Occupancies_OccupantId",
                table: "Occupancies");

            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "TagRemoteRecords");

            migrationBuilder.DropColumn(
                name: "OccupantId",
                table: "Occupancies");

            migrationBuilder.AddColumn<int>(
                name: "OccupancyId",
                table: "Persons",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Units_TagRemoteRecordId",
                table: "Units",
                column: "TagRemoteRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_Persons_OccupancyId",
                table: "Persons",
                column: "OccupancyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Persons_Occupancies_OccupancyId",
                table: "Persons",
                column: "OccupancyId",
                principalTable: "Occupancies",
                principalColumn: "OccupancyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Units_TagRemoteRecords_TagRemoteRecordId",
                table: "Units",
                column: "TagRemoteRecordId",
                principalTable: "TagRemoteRecords",
                principalColumn: "TagRemoteRecordId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
