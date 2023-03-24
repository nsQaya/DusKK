using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TDV.Migrations
{
    public partial class reMaked_country : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Countries",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "Countries",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "Countries");
        }
    }
}
