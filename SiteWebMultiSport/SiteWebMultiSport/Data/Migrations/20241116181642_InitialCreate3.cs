using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SiteWebMultiSport.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "phone",
                table: "Adherants",
                newName: "Phone");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "Adherants",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "date_naissance",
                table: "Adherants",
                newName: "DateNaissance");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "Adherants",
                newName: "phone");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Adherants",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "DateNaissance",
                table: "Adherants",
                newName: "date_naissance");
        }
    }
}
