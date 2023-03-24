using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TDV.Migrations
{
    public partial class Rmoved_FuneralFlight_FuneralID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FuneralId",
                table: "FuneralFlights",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_FuneralFlights_FuneralId",
                table: "FuneralFlights",
                column: "FuneralId");

            migrationBuilder.AddForeignKey(
                name: "FK_FuneralFlights_Funerals_FuneralId",
                table: "FuneralFlights",
                column: "FuneralId",
                principalTable: "Funerals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
