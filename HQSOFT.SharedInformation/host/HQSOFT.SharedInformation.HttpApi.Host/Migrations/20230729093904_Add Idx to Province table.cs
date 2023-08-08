using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HQSOFT.SharedInformation.Migrations
{
    /// <inheritdoc />
    public partial class AddIdxtoProvincetable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SIFCountries_CountryCode",
                table: "SIFCountries");

            migrationBuilder.AddColumn<int>(
                name: "Idx",
                table: "SIFProvinces",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Idx",
                table: "SIFProvinces");

            migrationBuilder.CreateIndex(
                name: "IX_SIFCountries_CountryCode",
                table: "SIFCountries",
                column: "CountryCode",
                unique: true);
        }
    }
}
