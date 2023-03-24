using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TDV.Migrations
{
    public partial class Changed_Funeral_Props : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Funerals_AbpOrganizationUnits_GiverOrgUnitId",
                table: "Funerals");

            migrationBuilder.DropForeignKey(
                name: "FK_Funerals_AbpUsers_EmployeePersonId",
                table: "Funerals");

            migrationBuilder.AlterColumn<long>(
                name: "GiverOrgUnitId",
                table: "Funerals",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "EmployeePersonId",
                table: "Funerals",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_Funerals_AbpOrganizationUnits_GiverOrgUnitId",
                table: "Funerals",
                column: "GiverOrgUnitId",
                principalTable: "AbpOrganizationUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Funerals_AbpUsers_EmployeePersonId",
                table: "Funerals",
                column: "EmployeePersonId",
                principalTable: "AbpUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Funerals_AbpOrganizationUnits_GiverOrgUnitId",
                table: "Funerals");

            migrationBuilder.DropForeignKey(
                name: "FK_Funerals_AbpUsers_EmployeePersonId",
                table: "Funerals");

            migrationBuilder.AlterColumn<long>(
                name: "GiverOrgUnitId",
                table: "Funerals",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "EmployeePersonId",
                table: "Funerals",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Funerals_AbpOrganizationUnits_GiverOrgUnitId",
                table: "Funerals",
                column: "GiverOrgUnitId",
                principalTable: "AbpOrganizationUnits",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Funerals_AbpUsers_EmployeePersonId",
                table: "Funerals",
                column: "EmployeePersonId",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
