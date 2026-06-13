using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace CoreApplication.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddLocationTracking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CourierWorkSession",
                columns: table => new
                {
                    CourierWorkSessionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FleetId = table.Column<int>(type: "int", nullable: false),
                    CourierId = table.Column<int>(type: "int", nullable: false),
                    SessionDate = table.Column<DateOnly>(type: "date", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TotalDistanceMeters = table.Column<double>(type: "float", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: false),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourierWorkSession", x => x.CourierWorkSessionId);
                    table.ForeignKey(
                        name: "FK_CourierWorkSession_Courier_CourierId",
                        column: x => x.CourierId,
                        principalTable: "Courier",
                        principalColumn: "CourierId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CourierWorkSession_Fleets_FleetId",
                        column: x => x.FleetId,
                        principalTable: "Fleets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CourierLocationTrack",
                columns: table => new
                {
                    CourierLocationTrackId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkSessionId = table.Column<int>(type: "int", nullable: false),
                    FleetId = table.Column<int>(type: "int", nullable: false),
                    CourierId = table.Column<int>(type: "int", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    Location = table.Column<Point>(type: "geography", nullable: false),
                    Accuracy = table.Column<float>(type: "real", nullable: true),
                    Speed = table.Column<float>(type: "real", nullable: true),
                    Heading = table.Column<float>(type: "real", nullable: true),
                    RecordedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourierLocationTrack", x => x.CourierLocationTrackId);
                    table.ForeignKey(
                        name: "FK_CourierLocationTrack_CourierWorkSession_WorkSessionId",
                        column: x => x.WorkSessionId,
                        principalTable: "CourierWorkSession",
                        principalColumn: "CourierWorkSessionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourierLocationTrack_CourierId_RecordedAt",
                table: "CourierLocationTrack",
                columns: new[] { "CourierId", "RecordedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_CourierLocationTrack_FleetId_RecordedAt",
                table: "CourierLocationTrack",
                columns: new[] { "FleetId", "RecordedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_CourierLocationTrack_WorkSessionId",
                table: "CourierLocationTrack",
                column: "WorkSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_CourierWorkSession_CourierId_SessionDate",
                table: "CourierWorkSession",
                columns: new[] { "CourierId", "SessionDate" });

            migrationBuilder.CreateIndex(
                name: "IX_CourierWorkSession_FleetId_SessionDate",
                table: "CourierWorkSession",
                columns: new[] { "FleetId", "SessionDate" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourierLocationTrack");

            migrationBuilder.DropTable(
                name: "CourierWorkSession");
        }
    }
}
