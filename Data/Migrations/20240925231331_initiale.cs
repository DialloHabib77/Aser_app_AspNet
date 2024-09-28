using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace aser_electrification.Data.Migrations
{
    /// <inheritdoc />
    public partial class initiale : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    ID_Region = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom_Region = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.ID_Region);
                });

            migrationBuilder.CreateTable(
                name: "Departements",
                columns: table => new
                {
                    ID_Departement = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom_Departement = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ID_Region = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departements", x => x.ID_Departement);
                    table.ForeignKey(
                        name: "FK_Departements_Regions_ID_Region",
                        column: x => x.ID_Region,
                        principalTable: "Regions",
                        principalColumn: "ID_Region",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Communes",
                columns: table => new
                {
                    ID_Commune = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom_Commune = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ID_Departement = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Communes", x => x.ID_Commune);
                    table.ForeignKey(
                        name: "FK_Communes_Departements_ID_Departement",
                        column: x => x.ID_Departement,
                        principalTable: "Departements",
                        principalColumn: "ID_Departement",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Villages",
                columns: table => new
                {
                    ID_Village = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom_Village = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Identifiant_Village = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Longitude = table.Column<float>(type: "real", nullable: false),
                    Latitude = table.Column<float>(type: "real", nullable: false),
                    Nombre_Menages = table.Column<int>(type: "int", nullable: false),
                    Statut_Electrification = table.Column<bool>(type: "bit", nullable: false),
                    ID_Commune = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Villages", x => x.ID_Village);
                    table.ForeignKey(
                        name: "FK_Villages_Communes_ID_Commune",
                        column: x => x.ID_Commune,
                        principalTable: "Communes",
                        principalColumn: "ID_Commune",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Recensements",
                columns: table => new
                {
                    ID_Recensement = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date_Recensement = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Nombre_Menages_Electrifies = table.Column<int>(type: "int", nullable: false),
                    ID_Village = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recensements", x => x.ID_Recensement);
                    table.ForeignKey(
                        name: "FK_Recensements_Villages_ID_Village",
                        column: x => x.ID_Village,
                        principalTable: "Villages",
                        principalColumn: "ID_Village",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Communes_ID_Departement",
                table: "Communes",
                column: "ID_Departement");

            migrationBuilder.CreateIndex(
                name: "IX_Departements_ID_Region",
                table: "Departements",
                column: "ID_Region");

            migrationBuilder.CreateIndex(
                name: "IX_Recensements_ID_Village",
                table: "Recensements",
                column: "ID_Village");

            migrationBuilder.CreateIndex(
                name: "IX_Villages_ID_Commune",
                table: "Villages",
                column: "ID_Commune");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Recensements");

            migrationBuilder.DropTable(
                name: "Villages");

            migrationBuilder.DropTable(
                name: "Communes");

            migrationBuilder.DropTable(
                name: "Departements");

            migrationBuilder.DropTable(
                name: "Regions");
        }
    }
}
