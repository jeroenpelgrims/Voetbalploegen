using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Voetbalploegen.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clubs",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Opgericht = table.Column<int>(type: "INTEGER", nullable: true),
                    Aansluitingsjaar = table.Column<int>(type: "INTEGER", nullable: true),
                    Stadion = table.Column<string>(type: "TEXT", nullable: true),
                    Website = table.Column<string>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    Telefoon = table.Column<string>(type: "TEXT", nullable: true),
                    Sponsors = table.Column<string>(type: "TEXT", nullable: true),
                    Budget = table.Column<int>(type: "INTEGER", nullable: true),
                    Bijnamen = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clubs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Spelers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Voornaam = table.Column<string>(type: "TEXT", nullable: false),
                    Naam = table.Column<string>(type: "TEXT", nullable: false),
                    Nationaliteit = table.Column<string>(type: "TEXT", nullable: false),
                    Geboortedatum = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ClubId = table.Column<string>(type: "TEXT", nullable: false),
                    Positie = table.Column<string>(type: "TEXT", nullable: false),
                    Rechtsvoetig = table.Column<bool>(type: "INTEGER", nullable: false),
                    Wedstrijden = table.Column<int>(type: "INTEGER", nullable: false),
                    AantalKeerTitularis = table.Column<int>(type: "INTEGER", nullable: false),
                    AantalKeerVervangen = table.Column<int>(type: "INTEGER", nullable: false),
                    AantalGespeeldeMinuten = table.Column<int>(type: "INTEGER", nullable: false),
                    Goals = table.Column<int>(type: "INTEGER", nullable: false),
                    Assists = table.Column<int>(type: "INTEGER", nullable: false),
                    AantalRodeKaarten = table.Column<int>(type: "INTEGER", nullable: false),
                    AantalGeleKaarten = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Spelers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Spelers_Clubs_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Clubs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Wedstrijden",
                columns: table => new
                {
                    ThuisploegId = table.Column<string>(type: "TEXT", nullable: false),
                    BezoekersId = table.Column<string>(type: "TEXT", nullable: false),
                    ThuisploegScore = table.Column<int>(type: "INTEGER", nullable: false),
                    BezoekersScore = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wedstrijden", x => new { x.ThuisploegId, x.BezoekersId, x.ThuisploegScore, x.BezoekersScore });
                    table.ForeignKey(
                        name: "FK_Wedstrijden_Clubs_BezoekersId",
                        column: x => x.BezoekersId,
                        principalTable: "Clubs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Wedstrijden_Clubs_ThuisploegId",
                        column: x => x.ThuisploegId,
                        principalTable: "Clubs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Spelers_ClubId",
                table: "Spelers",
                column: "ClubId");

            migrationBuilder.CreateIndex(
                name: "IX_Wedstrijden_BezoekersId",
                table: "Wedstrijden",
                column: "BezoekersId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Spelers");

            migrationBuilder.DropTable(
                name: "Wedstrijden");

            migrationBuilder.DropTable(
                name: "Clubs");
        }
    }
}
