using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace assignment3.Migrations
{
    /// <inheritdoc />
    public partial class FixDateTypesAndSync : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bodies",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Dist = table.Column<float>(type: "real", nullable: false),
                    BodyType = table.Column<int>(type: "int", nullable: false),
                    ParentPlanetName = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bodies", x => x.Name);
                    table.ForeignKey(
                        name: "FK_Bodies_Bodies_ParentPlanetName",
                        column: x => x.ParentPlanetName,
                        principalTable: "Bodies",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LaunchPads",
                columns: table => new
                {
                    LocationId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaxWeight = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LaunchPads", x => x.LocationId);
                });

            migrationBuilder.CreateTable(
                name: "Rockets",
                columns: table => new
                {
                    RocketId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Weight = table.Column<float>(type: "real", nullable: false),
                    FuelCap = table.Column<int>(type: "int", nullable: false),
                    Payload = table.Column<int>(type: "int", nullable: false),
                    Stages = table.Column<int>(type: "int", nullable: false),
                    CrewCap = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rockets", x => x.RocketId);
                });

            migrationBuilder.CreateTable(
                name: "Staff",
                columns: table => new
                {
                    StaffId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    HireDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PayGrade = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Staff", x => x.StaffId);
                });

            migrationBuilder.CreateTable(
                name: "Astronauts",
                columns: table => new
                {
                    StaffId = table.Column<int>(type: "int", nullable: false),
                    Rank = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExperienceSim = table.Column<float>(type: "real", nullable: false),
                    ExperienceSpace = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Astronauts", x => x.StaffId);
                    table.ForeignKey(
                        name: "FK_Astronauts_Staff_StaffId",
                        column: x => x.StaffId,
                        principalTable: "Staff",
                        principalColumn: "StaffId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Managers",
                columns: table => new
                {
                    StaffId = table.Column<int>(type: "int", nullable: false),
                    Department = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Managers", x => x.StaffId);
                    table.ForeignKey(
                        name: "FK_Managers_Staff_StaffId",
                        column: x => x.StaffId,
                        principalTable: "Staff",
                        principalColumn: "StaffId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Scientists",
                columns: table => new
                {
                    StaffId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Speciality = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scientists", x => x.StaffId);
                    table.ForeignKey(
                        name: "FK_Scientists_Staff_StaffId",
                        column: x => x.StaffId,
                        principalTable: "Staff",
                        principalColumn: "StaffId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Missions",
                columns: table => new
                {
                    MissionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LaunchDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Duration = table.Column<float>(type: "real", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    RocketId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    LaunchLocation = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CelestialDest = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ManagerId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Missions", x => x.MissionId);
                    table.ForeignKey(
                        name: "FK_Missions_Bodies_CelestialDest",
                        column: x => x.CelestialDest,
                        principalTable: "Bodies",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Missions_LaunchPads_LaunchLocation",
                        column: x => x.LaunchLocation,
                        principalTable: "LaunchPads",
                        principalColumn: "LocationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Missions_Managers_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "Managers",
                        principalColumn: "StaffId");
                    table.ForeignKey(
                        name: "FK_Missions_Rockets_RocketId",
                        column: x => x.RocketId,
                        principalTable: "Rockets",
                        principalColumn: "RocketId");
                });

            migrationBuilder.CreateTable(
                name: "AstronautMission",
                columns: table => new
                {
                    MissionId = table.Column<int>(type: "int", nullable: false),
                    StaffId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AstronautMission", x => new { x.MissionId, x.StaffId });
                    table.ForeignKey(
                        name: "FK_AstronautMission_Astronauts_StaffId",
                        column: x => x.StaffId,
                        principalTable: "Astronauts",
                        principalColumn: "StaffId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AstronautMission_Missions_MissionId",
                        column: x => x.MissionId,
                        principalTable: "Missions",
                        principalColumn: "MissionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScientistMission",
                columns: table => new
                {
                    MissionId = table.Column<int>(type: "int", nullable: false),
                    StaffId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScientistMission", x => new { x.MissionId, x.StaffId });
                    table.ForeignKey(
                        name: "FK_ScientistMission_Missions_MissionId",
                        column: x => x.MissionId,
                        principalTable: "Missions",
                        principalColumn: "MissionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScientistMission_Scientists_StaffId",
                        column: x => x.StaffId,
                        principalTable: "Scientists",
                        principalColumn: "StaffId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Bodies",
                columns: new[] { "Name", "BodyType", "Dist", "ParentPlanetName" },
                values: new object[,]
                {
                    { "Earth", 0, 1f, null },
                    { "Jupiter", 1, 5.2f, null }
                });

            migrationBuilder.InsertData(
                table: "LaunchPads",
                columns: new[] { "LocationId", "MaxWeight", "Status" },
                values: new object[,]
                {
                    { "Pad-Alpha", 1000000, "Active" },
                    { "Pad-Beta", 800000, "Active" }
                });

            migrationBuilder.InsertData(
                table: "Rockets",
                columns: new[] { "RocketId", "CrewCap", "FuelCap", "Name", "Payload", "Stages", "Weight" },
                values: new object[,]
                {
                    { "SN-01", 5, 4800000, "Falcon-X", 140000, 2, 5000000f },
                    { "SN-02", 3, 2100000, "Saturn-V", 130000, 3, 2800000f }
                });

            migrationBuilder.InsertData(
                table: "Staff",
                columns: new[] { "StaffId", "HireDate", "Name", "PayGrade" },
                values: new object[,]
                {
                    { 1, new DateTime(2016, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Rachel Adams", 2 },
                    { 2, new DateTime(2010, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Jane Smith", 4 },
                    { 101, new DateTime(2020, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Jessica Day", 5 },
                    { 102, new DateTime(2018, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nick Miller", 6 },
                    { 103, new DateTime(2012, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ali Hazelwood", 7 },
                    { 201, new DateTime(2016, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Rick Roll", 8 },
                    { 202, new DateTime(2011, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Amanda Smith", 9 },
                    { 203, new DateTime(2009, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Damon Salvatore", 9 },
                    { 204, new DateTime(2023, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bella Hadid", 7 },
                    { 205, new DateTime(2020, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bridget Mendler", 8 }
                });

            migrationBuilder.InsertData(
                table: "Astronauts",
                columns: new[] { "StaffId", "ExperienceSim", "ExperienceSpace", "Rank" },
                values: new object[,]
                {
                    { 201, 4800f, 4300f, "Pilot" },
                    { 202, 7200f, 8600f, "Commander" },
                    { 203, 8500f, 10500f, "Commander" },
                    { 204, 1500f, 0f, "Astronaut Candidate" },
                    { 205, 3200f, 4100f, "Mission Specialt" }
                });

            migrationBuilder.InsertData(
                table: "Bodies",
                columns: new[] { "Name", "BodyType", "Dist", "ParentPlanetName" },
                values: new object[,]
                {
                    { "Europa", 3, 5.201f, "Jupiter" },
                    { "Lunar", 3, 1.002f, "Earth" }
                });

            migrationBuilder.InsertData(
                table: "Managers",
                columns: new[] { "StaffId", "Department" },
                values: new object[,]
                {
                    { 1, "Ground Support" },
                    { 2, "Flight Ops" }
                });

            migrationBuilder.InsertData(
                table: "Scientists",
                columns: new[] { "StaffId", "Speciality", "Title" },
                values: new object[,]
                {
                    { 101, "Biology", "Phd" },
                    { 102, "Physics", "Dr" },
                    { 103, "Astronomy", "Proffesor" }
                });

            migrationBuilder.InsertData(
                table: "Missions",
                columns: new[] { "MissionId", "CelestialDest", "Duration", "LaunchDate", "LaunchLocation", "ManagerId", "Name", "RocketId", "Status", "Type" },
                values: new object[,]
                {
                    { 1, "Lunar", 27f, new DateTime(2034, 3, 16, 9, 0, 0, 0, DateTimeKind.Unspecified), "Pad-Beta", 2, "Mission X", "SN-01", 3, 1 },
                    { 2, "Jupiter", 14.5f, new DateTime(2045, 7, 1, 10, 0, 0, 0, DateTimeKind.Unspecified), "Pad-Alpha", 1, "Apollo 11", "SN-02", 3, 2 }
                });

            migrationBuilder.InsertData(
                table: "AstronautMission",
                columns: new[] { "MissionId", "StaffId" },
                values: new object[,]
                {
                    { 1, 201 },
                    { 1, 202 },
                    { 1, 203 },
                    { 1, 204 },
                    { 1, 205 },
                    { 2, 202 },
                    { 2, 203 }
                });

            migrationBuilder.InsertData(
                table: "ScientistMission",
                columns: new[] { "MissionId", "StaffId" },
                values: new object[,]
                {
                    { 1, 101 },
                    { 1, 102 },
                    { 2, 101 },
                    { 2, 102 },
                    { 2, 103 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AstronautMission_StaffId",
                table: "AstronautMission",
                column: "StaffId");

            migrationBuilder.CreateIndex(
                name: "IX_Bodies_ParentPlanetName",
                table: "Bodies",
                column: "ParentPlanetName");

            migrationBuilder.CreateIndex(
                name: "IX_Missions_CelestialDest",
                table: "Missions",
                column: "CelestialDest");

            migrationBuilder.CreateIndex(
                name: "IX_Missions_LaunchLocation",
                table: "Missions",
                column: "LaunchLocation");

            migrationBuilder.CreateIndex(
                name: "IX_Missions_ManagerId",
                table: "Missions",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Missions_RocketId",
                table: "Missions",
                column: "RocketId",
                unique: true,
                filter: "[RocketId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ScientistMission_StaffId",
                table: "ScientistMission",
                column: "StaffId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AstronautMission");

            migrationBuilder.DropTable(
                name: "ScientistMission");

            migrationBuilder.DropTable(
                name: "Astronauts");

            migrationBuilder.DropTable(
                name: "Missions");

            migrationBuilder.DropTable(
                name: "Scientists");

            migrationBuilder.DropTable(
                name: "Bodies");

            migrationBuilder.DropTable(
                name: "LaunchPads");

            migrationBuilder.DropTable(
                name: "Managers");

            migrationBuilder.DropTable(
                name: "Rockets");

            migrationBuilder.DropTable(
                name: "Staff");
        }
    }
}
