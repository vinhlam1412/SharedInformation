using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HQSOFT.SharedInformation.Migrations
{
    /// <inheritdoc />
    public partial class AddIdxtoCountrytable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "SIFCountries");

            migrationBuilder.AddColumn<int>(
                name: "Idx",
                table: "SIFCountries",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Idx",
                table: "SIFCountries");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "SIFCountries",
                type: "text",
                nullable: true);
        }
    }
}
