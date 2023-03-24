using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TDV.Migrations
{
    public partial class Added_FixedPrice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FixedPriceId",
                table: "FixedPriceDetails",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FixedPrices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_FixedPrices", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FixedPriceDetails_FixedPriceId",
                table: "FixedPriceDetails",
                column: "FixedPriceId");

            migrationBuilder.AddForeignKey(
                name: "FK_FixedPriceDetails_FixedPrices_FixedPriceId",
                table: "FixedPriceDetails",
                column: "FixedPriceId",
                principalTable: "FixedPrices",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FixedPriceDetails_FixedPrices_FixedPriceId",
                table: "FixedPriceDetails");

            migrationBuilder.DropTable(
                name: "FixedPrices");

            migrationBuilder.DropIndex(
                name: "IX_FixedPriceDetails_FixedPriceId",
                table: "FixedPriceDetails");

            migrationBuilder.DropColumn(
                name: "FixedPriceId",
                table: "FixedPriceDetails");
        }
    }
}
