using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RoeiJeRot.Database.Migrations
{
    public partial class InitialMigrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "sailing_boats",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    InService = table.Column<bool>(nullable: false),
                    RequiredLevel = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sailing_boats", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "sailing_competitions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sailing_competitions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    StreetName = table.Column<string>(nullable: true),
                    HouseNumber = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    SailingLevel = table.Column<int>(nullable: false),
                    Username = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "sailing_competition_participants",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SailingCompetitionId = table.Column<int>(nullable: false),
                    ParticipantId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sailing_competition_participants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_sailing_competition_participants_sailing_boats_ParticipantId",
                        column: x => x.ParticipantId,
                        principalTable: "sailing_boats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_sailing_competition_participants_sailing_competitions_SailingCompetitionId",
                        column: x => x.SailingCompetitionId,
                        principalTable: "sailing_competitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "sailing_boat_damage_reports",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DamagedSailingBoatId = table.Column<int>(nullable: false),
                    DamagedById = table.Column<int>(nullable: false),
                    DamagedAtDate = table.Column<DateTime>(nullable: false),
                    DamageFixedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sailing_boat_damage_reports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_sailing_boat_damage_reports_users_DamagedById",
                        column: x => x.DamagedById,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_sailing_boat_damage_reports_sailing_boats_DamagedSailingBoatId",
                        column: x => x.DamagedSailingBoatId,
                        principalTable: "sailing_boats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "sailing_reservations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(nullable: false),
                    Duration = table.Column<byte>(nullable: false),
                    ReservedByUserId = table.Column<int>(nullable: false),
                    ReservedSailingBoatId = table.Column<int>(nullable: false),
                    ReservedById = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sailing_reservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_sailing_reservations_users_ReservedById",
                        column: x => x.ReservedById,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_sailing_reservations_sailing_boats_ReservedSailingBoatId",
                        column: x => x.ReservedSailingBoatId,
                        principalTable: "sailing_boats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_sailing_boat_damage_reports_DamagedById",
                table: "sailing_boat_damage_reports",
                column: "DamagedById");

            migrationBuilder.CreateIndex(
                name: "IX_sailing_boat_damage_reports_DamagedSailingBoatId",
                table: "sailing_boat_damage_reports",
                column: "DamagedSailingBoatId");

            migrationBuilder.CreateIndex(
                name: "IX_sailing_competition_participants_ParticipantId",
                table: "sailing_competition_participants",
                column: "ParticipantId");

            migrationBuilder.CreateIndex(
                name: "IX_sailing_competition_participants_SailingCompetitionId",
                table: "sailing_competition_participants",
                column: "SailingCompetitionId");

            migrationBuilder.CreateIndex(
                name: "IX_sailing_reservations_ReservedById",
                table: "sailing_reservations",
                column: "ReservedById");

            migrationBuilder.CreateIndex(
                name: "IX_sailing_reservations_ReservedSailingBoatId",
                table: "sailing_reservations",
                column: "ReservedSailingBoatId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "sailing_boat_damage_reports");

            migrationBuilder.DropTable(
                name: "sailing_competition_participants");

            migrationBuilder.DropTable(
                name: "sailing_reservations");

            migrationBuilder.DropTable(
                name: "sailing_competitions");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "sailing_boats");
        }
    }
}
