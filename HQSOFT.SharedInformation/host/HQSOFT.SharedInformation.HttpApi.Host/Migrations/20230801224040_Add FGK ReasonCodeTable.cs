using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HQSOFT.SharedInformation.Migrations
{
    /// <inheritdoc />
    public partial class AddFGKReasonCodeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_SIFReasonCodes_AccountId",
                table: "SIFReasonCodes",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_SIFReasonCodes_GLAccounts_AccountId",
                table: "SIFReasonCodes",
                column: "AccountId",
                principalTable: "GLAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SIFReasonCodes_GLAccounts_AccountId",
                table: "SIFReasonCodes");

            migrationBuilder.DropIndex(
                name: "IX_SIFReasonCodes_AccountId",
                table: "SIFReasonCodes");
        }
    }
}
