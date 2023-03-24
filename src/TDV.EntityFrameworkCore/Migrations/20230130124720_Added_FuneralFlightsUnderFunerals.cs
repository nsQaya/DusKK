using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TDV.Migrations
{
    public partial class Added_FuneralFlightsUnderFunerals : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FuneralId",
                table: "FuneralFlights",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FuneralFlights_FuneralId",
                table: "FuneralFlights",
                column: "FuneralId");

            migrationBuilder.AddForeignKey(
                name: "FK_FuneralFlights_Funerals_FuneralId",
                table: "FuneralFlights",
                column: "FuneralId",
                principalTable: "Funerals",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FuneralFlights_Funerals_FuneralId",
                table: "FuneralFlights");

            migrationBuilder.DropIndex(
                name: "IX_FuneralFlights_FuneralId",
                table: "FuneralFlights");

            migrationBuilder.DropColumn(
                name: "FuneralId",
                table: "FuneralFlights");
        }
    }
}
