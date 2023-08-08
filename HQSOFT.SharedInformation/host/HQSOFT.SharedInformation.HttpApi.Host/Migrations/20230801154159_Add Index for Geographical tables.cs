using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HQSOFT.SharedInformation.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexforGeographicaltables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_SIFWards_WardName",
                table: "SIFWards",
                column: "WardName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SIFStates_StateCode",
                table: "SIFStates",
                column: "StateCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SIFProvinces_ProvinceCode",
                table: "SIFProvinces",
                column: "ProvinceCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SIFDistricts_DistrictName",
                table: "SIFDistricts",
                column: "DistrictName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SIFCountries_CountryCode",
                table: "SIFCountries",
                column: "CountryCode",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SIFWards_WardName",
                table: "SIFWards");

            migrationBuilder.DropIndex(
                name: "IX_SIFStates_StateCode",
                table: "SIFStates");

            migrationBuilder.DropIndex(
                name: "IX_SIFProvinces_ProvinceCode",
                table: "SIFProvinces");

            migrationBuilder.DropIndex(
                name: "IX_SIFDistricts_DistrictName",
                table: "SIFDistricts");

            migrationBuilder.DropIndex(
                name: "IX_SIFCountries_CountryCode",
                table: "SIFCountries");
        }
    }
}
