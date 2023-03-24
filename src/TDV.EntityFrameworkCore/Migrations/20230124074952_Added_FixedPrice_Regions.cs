using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TDV.Migrations
{
    public partial class Added_FixedPrice_Regions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FixedPriceId",
                table: "Regions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Regions_FixedPriceId",
                table: "Regions",
                column: "FixedPriceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Regions_FixedPrices_FixedPriceId",
                table: "Regions",
                column: "FixedPriceId",
                principalTable: "FixedPrices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Regions_FixedPrices_FixedPriceId",
                table: "Regions");

            migrationBuilder.DropIndex(
                name: "IX_Regions_FixedPriceId",
                table: "Regions");

            migrationBuilder.DropColumn(
                name: "FixedPriceId",
                table: "Regions");
        }
    }
}
