using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HQSOFT.SharedInformation.Migrations
{
    /// <inheritdoc />
    public partial class AddGeographyTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SIFCountries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CountryCode = table.Column<string>(type: "text", nullable: false),
                    CountryName = table.Column<string>(type: "text", nullable: false),
                    DateFormat = table.Column<string>(type: "text", nullable: true),
                    TimeFormat = table.Column<string>(type: "text", nullable: true),
                    TimeZone = table.Column<string>(type: "text", nullable: true),
                    Code = table.Column<string>(type: "text", nullable: true),
                    ExtraProperties = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SIFCountries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SIFProvinces",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProvinceCode = table.Column<string>(type: "text", nullable: false),
                    ProvinceName = table.Column<string>(type: "text", nullable: false),
                    CountryId = table.Column<Guid>(type: "uuid", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SIFProvinces", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SIFProvinces_SIFCountries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "SIFCountries",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SIFStates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StateCode = table.Column<string>(type: "text", nullable: false),
                    StateName = table.Column<string>(type: "text", nullable: false),
                    CountryId = table.Column<Guid>(type: "uuid", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SIFStates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SIFStates_SIFCountries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "SIFCountries",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SIFDistricts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DistrictName = table.Column<string>(type: "text", nullable: false),
                    ProvinceId = table.Column<Guid>(type: "uuid", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SIFDistricts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SIFDistricts_SIFProvinces_ProvinceId",
                        column: x => x.ProvinceId,
                        principalTable: "SIFProvinces",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SIFWards",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WardName = table.Column<string>(type: "text", nullable: false),
                    DistrictId = table.Column<Guid>(type: "uuid", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SIFWards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SIFWards_SIFDistricts_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "SIFDistricts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SIFDistricts_ProvinceId",
                table: "SIFDistricts",
                column: "ProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_SIFProvinces_CountryId",
                table: "SIFProvinces",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_SIFStates_CountryId",
                table: "SIFStates",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_SIFWards_DistrictId",
                table: "SIFWards",
                column: "DistrictId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SIFStates");

            migrationBuilder.DropTable(
                name: "SIFWards");

            migrationBuilder.DropTable(
                name: "SIFDistricts");

            migrationBuilder.DropTable(
                name: "SIFProvinces");

            migrationBuilder.DropTable(
                name: "SIFCountries");
        }
    }
}
