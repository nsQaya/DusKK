using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TDV.Migrations
{
    public partial class remaked_funeral : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Funerals_AbpOrganizationUnits_OrganizationUnitId",
                table: "Funerals");

            migrationBuilder.RenameColumn(
                name: "Statu",
                table: "Funerals",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "OrganizationUnitId",
                table: "Funerals",
                newName: "GiverOrgUnitId");

            migrationBuilder.RenameIndex(
                name: "IX_Funerals_OrganizationUnitId",
                table: "Funerals",
                newName: "IX_Funerals_GiverOrgUnitId");

            migrationBuilder.AddColumn<long>(
                name: "ContractorOrgUnitId",
                table: "Funerals",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "EmployeePersonId",
                table: "Funerals",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "FuneralPackageId",
                table: "Funerals",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "OperationDate",
                table: "Funerals",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "OwnerOrgUnitId",
                table: "Funerals",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Funerals_ContractorOrgUnitId",
                table: "Funerals",
                column: "ContractorOrgUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Funerals_EmployeePersonId",
                table: "Funerals",
                column: "EmployeePersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Funerals_FuneralPackageId",
                table: "Funerals",
                column: "FuneralPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_Funerals_OwnerOrgUnitId",
                table: "Funerals",
                column: "OwnerOrgUnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Funerals_AbpOrganizationUnits_ContractorOrgUnitId",
                table: "Funerals",
                column: "ContractorOrgUnitId",
                principalTable: "AbpOrganizationUnits",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Funerals_AbpOrganizationUnits_GiverOrgUnitId",
                table: "Funerals",
                column: "GiverOrgUnitId",
                principalTable: "AbpOrganizationUnits",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Funerals_AbpOrganizationUnits_OwnerOrgUnitId",
                table: "Funerals",
                column: "OwnerOrgUnitId",
                principalTable: "AbpOrganizationUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Funerals_AbpUsers_EmployeePersonId",
                table: "Funerals",
                column: "EmployeePersonId",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Funerals_FuneralPackages_FuneralPackageId",
                table: "Funerals",
                column: "FuneralPackageId",
                principalTable: "FuneralPackages",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Funerals_AbpOrganizationUnits_ContractorOrgUnitId",
                table: "Funerals");

            migrationBuilder.DropForeignKey(
                name: "FK_Funerals_AbpOrganizationUnits_GiverOrgUnitId",
                table: "Funerals");

            migrationBuilder.DropForeignKey(
                name: "FK_Funerals_AbpOrganizationUnits_OwnerOrgUnitId",
                table: "Funerals");

            migrationBuilder.DropForeignKey(
                name: "FK_Funerals_AbpUsers_EmployeePersonId",
                table: "Funerals");

            migrationBuilder.DropForeignKey(
                name: "FK_Funerals_FuneralPackages_FuneralPackageId",
                table: "Funerals");

            migrationBuilder.DropIndex(
                name: "IX_Funerals_ContractorOrgUnitId",
                table: "Funerals");

            migrationBuilder.DropIndex(
                name: "IX_Funerals_EmployeePersonId",
                table: "Funerals");

            migrationBuilder.DropIndex(
                name: "IX_Funerals_FuneralPackageId",
                table: "Funerals");

            migrationBuilder.DropIndex(
                name: "IX_Funerals_OwnerOrgUnitId",
                table: "Funerals");

            migrationBuilder.DropColumn(
                name: "ContractorOrgUnitId",
                table: "Funerals");

            migrationBuilder.DropColumn(
                name: "EmployeePersonId",
                table: "Funerals");

            migrationBuilder.DropColumn(
                name: "FuneralPackageId",
                table: "Funerals");

            migrationBuilder.DropColumn(
                name: "OperationDate",
                table: "Funerals");

            migrationBuilder.DropColumn(
                name: "OwnerOrgUnitId",
                table: "Funerals");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Funerals",
                newName: "Statu");

            migrationBuilder.RenameColumn(
                name: "GiverOrgUnitId",
                table: "Funerals",
                newName: "OrganizationUnitId");

            migrationBuilder.RenameIndex(
                name: "IX_Funerals_GiverOrgUnitId",
                table: "Funerals",
                newName: "IX_Funerals_OrganizationUnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Funerals_AbpOrganizationUnits_OrganizationUnitId",
                table: "Funerals",
                column: "OrganizationUnitId",
                principalTable: "AbpOrganizationUnits",
                principalColumn: "Id");
        }
    }
}
