using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TDV.Migrations
{
    public partial class remaked_userDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "UserDetails");

            migrationBuilder.AddColumn<int>(
                name: "ContactId",
                table: "UserDetails",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserDetails_ContactId",
                table: "UserDetails",
                column: "ContactId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserDetails_Contacts_ContactId",
                table: "UserDetails",
                column: "ContactId",
                principalTable: "Contacts",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserDetails_Contacts_ContactId",
                table: "UserDetails");

            migrationBuilder.DropIndex(
                name: "IX_UserDetails_ContactId",
                table: "UserDetails");

            migrationBuilder.DropColumn(
                name: "ContactId",
                table: "UserDetails");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "UserDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
