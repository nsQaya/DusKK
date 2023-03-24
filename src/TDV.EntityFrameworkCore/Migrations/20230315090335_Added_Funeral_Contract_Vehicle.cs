using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TDV.Migrations
{
    public partial class Added_Funeral_Contract_Vehicle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ContractId",
                table: "Funerals",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VehicleId",
                table: "Funerals",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Funerals_ContractId",
                table: "Funerals",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_Funerals_VehicleId",
                table: "Funerals",
                column: "VehicleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Funerals_Contracts_ContractId",
                table: "Funerals",
                column: "ContractId",
                principalTable: "Contracts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Funerals_Vehicles_VehicleId",
                table: "Funerals",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id");

            migrationBuilder.AddColumn<int>(
               name: "CompanyId",
               table: "Contracts",
               type: "int",
               nullable: false,
               defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_CompanyId",
                table: "Contracts",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_Companies_CompanyId",
                table: "Contracts",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Funerals_Contracts_ContractId",
                table: "Funerals");

            migrationBuilder.DropForeignKey(
                name: "FK_Funerals_Vehicles_VehicleId",
                table: "Funerals");

            migrationBuilder.DropIndex(
                name: "IX_Funerals_ContractId",
                table: "Funerals");

            migrationBuilder.DropIndex(
                name: "IX_Funerals_VehicleId",
                table: "Funerals");

            migrationBuilder.DropColumn(
                name: "ContractId",
                table: "Funerals");

            migrationBuilder.DropColumn(
                name: "VehicleId",
                table: "Funerals");

            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Companies_CompanyId",
                table: "Contracts");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_CompanyId",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Contracts");
        }
    }
}
