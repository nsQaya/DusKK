using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TDV.Migrations
{
    public partial class Added_FuneralFlights : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FuneralFlights",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    No = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Code = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    LiftOffDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LandingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FuneralId = table.Column<int>(type: "int", nullable: false),
                    AirlineCompanyId = table.Column<int>(type: "int", nullable: false),
                    LiftOffAirportId = table.Column<int>(type: "int", nullable: false),
                    LangingAirportId = table.Column<int>(type: "int", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FuneralFlights", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FuneralFlights_AirlineCompanies_AirlineCompanyId",
                        column: x => x.AirlineCompanyId,
                        principalTable: "AirlineCompanies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FuneralFlights_Airports_LangingAirportId",
                        column: x => x.LangingAirportId,
                        principalTable: "Airports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FuneralFlights_Airports_LiftOffAirportId",
                        column: x => x.LiftOffAirportId,
                        principalTable: "Airports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_FuneralFlights_Funerals_FuneralId",
                        column: x => x.FuneralId,
                        principalTable: "Funerals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FuneralFlights_AirlineCompanyId",
                table: "FuneralFlights",
                column: "AirlineCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_FuneralFlights_FuneralId",
                table: "FuneralFlights",
                column: "FuneralId");

            migrationBuilder.CreateIndex(
                name: "IX_FuneralFlights_LangingAirportId",
                table: "FuneralFlights",
                column: "LangingAirportId");

            migrationBuilder.CreateIndex(
                name: "IX_FuneralFlights_LiftOffAirportId",
                table: "FuneralFlights",
                column: "LiftOffAirportId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FuneralFlights");
        }
    }
}
