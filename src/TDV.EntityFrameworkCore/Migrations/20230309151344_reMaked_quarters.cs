using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TDV.Migrations
{
    public partial class reMaked_quarters : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Quarters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "Quarters",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Quarters");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "Quarters");
        }
    }
}
