using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SiteWebMultiSport.Data.Migrations
{
    /// <inheritdoc />
    public partial class boolIsAdminToAdherant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAdmin",
                table: "Adherants",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAdmin",
                table: "Adherants");
        }
    }
}
