using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Disciplines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Disciplines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Adherants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateNaissance = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false),
                    IsEncadrant = table.Column<bool>(type: "bit", nullable: false),
                    CreneauId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Adherants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DisciplineId = table.Column<int>(type: "int", nullable: false),
                    EncadrantId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sections_Adherants_EncadrantId",
                        column: x => x.EncadrantId,
                        principalTable: "Adherants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sections_Disciplines_DisciplineId",
                        column: x => x.DisciplineId,
                        principalTable: "Disciplines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Creneaux",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Jour = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HeureDebut = table.Column<TimeSpan>(type: "time", nullable: false),
                    HeureFin = table.Column<TimeSpan>(type: "time", nullable: false),
                    Lieu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Capacite = table.Column<int>(type: "int", nullable: false),
                    SectionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Creneaux", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Creneaux_Sections_SectionId",
                        column: x => x.SectionId,
                        principalTable: "Sections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Adherants_CreneauId",
                table: "Adherants",
                column: "CreneauId");

            migrationBuilder.CreateIndex(
                name: "IX_Creneaux_SectionId",
                table: "Creneaux",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Sections_DisciplineId",
                table: "Sections",
                column: "DisciplineId");

            migrationBuilder.CreateIndex(
                name: "IX_Sections_EncadrantId",
                table: "Sections",
                column: "EncadrantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Adherants_Creneaux_CreneauId",
                table: "Adherants",
                column: "CreneauId",
                principalTable: "Creneaux",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Adherants_Creneaux_CreneauId",
                table: "Adherants");

            migrationBuilder.DropTable(
                name: "Creneaux");

            migrationBuilder.DropTable(
                name: "Sections");

            migrationBuilder.DropTable(
                name: "Adherants");

            migrationBuilder.DropTable(
                name: "Disciplines");
        }
    }
}
