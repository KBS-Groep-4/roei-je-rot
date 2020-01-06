using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RoeiJeRot.Database.Migrations
{
    public partial class InitialMigrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "boat_types",
                table => new
                {
                    Id = table.Column<int>()
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PossiblePassengers = table.Column<int>(),
                    RequiredLevel = table.Column<int>(),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_boat_types", x => x.Id); });

            migrationBuilder.CreateTable(
                "permissions",
                table => new
                {
                    Id = table.Column<int>()
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_permissions", x => x.Id); });

            migrationBuilder.CreateTable(
                "users",
                table => new
                {
                    Id = table.Column<int>()
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    StreetName = table.Column<string>(nullable: true),
                    HouseNumber = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    SailingLevel = table.Column<int>(),
                    Username = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_users", x => x.Id); });

            migrationBuilder.CreateTable(
                "sailing_boats",
                table => new
                {
                    Id = table.Column<int>()
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<int>(),
                    BoatTypeId = table.Column<int>()
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sailing_boats", x => x.Id);
                    table.ForeignKey(
                        "FK_sailing_boats_boat_types_BoatTypeId",
                        x => x.BoatTypeId,
                        "boat_types",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "user_permissions",
                table => new
                {
                    Id = table.Column<int>()
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PermissionId = table.Column<int>(),
                    UserId = table.Column<int>()
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_permissions", x => x.Id);
                    table.ForeignKey(
                        "FK_user_permissions_permissions_PermissionId",
                        x => x.PermissionId,
                        "permissions",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_user_permissions_users_UserId",
                        x => x.UserId,
                        "users",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "sailing_boat_damage_reports",
                table => new
                {
                    Id = table.Column<int>()
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DamagedSailingBoatId = table.Column<int>(),
                    DamagedById = table.Column<int>(),
                    DamagedAtDate = table.Column<DateTime>(),
                    DamageFixedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sailing_boat_damage_reports", x => x.Id);
                    table.ForeignKey(
                        "FK_sailing_boat_damage_reports_users_DamagedById",
                        x => x.DamagedById,
                        "users",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_sailing_boat_damage_reports_sailing_boats_DamagedSailingBoatId",
                        x => x.DamagedSailingBoatId,
                        "sailing_boats",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "sailing_reservations",
                table => new
                {
                    Id = table.Column<int>()
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(),
                    Duration = table.Column<TimeSpan>(),
                    ReservedByUserId = table.Column<int>(),
                    ReservedSailingBoatId = table.Column<int>()
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sailing_reservations", x => x.Id);
                    table.ForeignKey(
                        "FK_sailing_reservations_users_ReservedByUserId",
                        x => x.ReservedByUserId,
                        "users",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_sailing_reservations_sailing_boats_ReservedSailingBoatId",
                        x => x.ReservedSailingBoatId,
                        "sailing_boats",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "sailing_competitions",
                table => new
                {
                    Id = table.Column<int>()
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ReservationId = table.Column<int>()
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sailing_competitions", x => x.Id);
                    table.ForeignKey(
                        "FK_sailing_competitions_sailing_reservations_ReservationId",
                        x => x.ReservationId,
                        "sailing_reservations",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "sailing_competition_participants",
                table => new
                {
                    Id = table.Column<int>()
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SailingCompetitionId = table.Column<int>(),
                    ParticipantId = table.Column<int>()
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sailing_competition_participants", x => x.Id);
                    table.ForeignKey(
                        "FK_sailing_competition_participants_users_ParticipantId",
                        x => x.ParticipantId,
                        "users",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_sailing_competition_participants_sailing_competitions_SailingCompetitionId",
                        x => x.SailingCompetitionId,
                        "sailing_competitions",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                "IX_sailing_boat_damage_reports_DamagedById",
                "sailing_boat_damage_reports",
                "DamagedById");

            migrationBuilder.CreateIndex(
                "IX_sailing_boat_damage_reports_DamagedSailingBoatId",
                "sailing_boat_damage_reports",
                "DamagedSailingBoatId");

            migrationBuilder.CreateIndex(
                "IX_sailing_boats_BoatTypeId",
                "sailing_boats",
                "BoatTypeId");

            migrationBuilder.CreateIndex(
                "IX_sailing_competition_participants_ParticipantId",
                "sailing_competition_participants",
                "ParticipantId");

            migrationBuilder.CreateIndex(
                "IX_sailing_competition_participants_SailingCompetitionId",
                "sailing_competition_participants",
                "SailingCompetitionId");

            migrationBuilder.CreateIndex(
                "IX_sailing_competitions_ReservationId",
                "sailing_competitions",
                "ReservationId");

            migrationBuilder.CreateIndex(
                "IX_sailing_reservations_ReservedByUserId",
                "sailing_reservations",
                "ReservedByUserId");

            migrationBuilder.CreateIndex(
                "IX_sailing_reservations_ReservedSailingBoatId",
                "sailing_reservations",
                "ReservedSailingBoatId");

            migrationBuilder.CreateIndex(
                "IX_user_permissions_PermissionId",
                "user_permissions",
                "PermissionId");

            migrationBuilder.CreateIndex(
                "IX_user_permissions_UserId",
                "user_permissions",
                "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "sailing_boat_damage_reports");

            migrationBuilder.DropTable(
                "sailing_competition_participants");

            migrationBuilder.DropTable(
                "user_permissions");

            migrationBuilder.DropTable(
                "sailing_competitions");

            migrationBuilder.DropTable(
                "permissions");

            migrationBuilder.DropTable(
                "sailing_reservations");

            migrationBuilder.DropTable(
                "users");

            migrationBuilder.DropTable(
                "sailing_boats");

            migrationBuilder.DropTable(
                "boat_types");
        }
    }
}