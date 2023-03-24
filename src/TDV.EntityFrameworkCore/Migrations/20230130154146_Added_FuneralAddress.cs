using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TDV.Migrations
{
    public partial class Added_FuneralAddress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FuneralAddreses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    FuneralId = table.Column<int>(type: "int", nullable: false),
                    QuarterId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_FuneralAddreses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FuneralAddreses_Funerals_FuneralId",
                        column: x => x.FuneralId,
                        principalTable: "Funerals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FuneralAddreses_Quarters_QuarterId",
                        column: x => x.QuarterId,
                        principalTable: "Quarters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FuneralAddreses_FuneralId",
                table: "FuneralAddreses",
                column: "FuneralId");

            migrationBuilder.CreateIndex(
                name: "IX_FuneralAddreses_QuarterId",
                table: "FuneralAddreses",
                column: "QuarterId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FuneralAddreses");
        }
    }
}
