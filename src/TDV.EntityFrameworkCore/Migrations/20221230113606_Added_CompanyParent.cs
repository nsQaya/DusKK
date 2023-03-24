using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TDV.Migrations
{
    public partial class Added_CompanyParent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CompanyParents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ParentCompanyId = table.Column<int>(type: "int", nullable: false, defaultValue:0 ),
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
                    table.PrimaryKey("PK_CompanyParents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyParents_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);

                    table.ForeignKey(
                        name: "FK_CompanyParents_Companies_ParentCompanyId",
                        column: x => x.ParentCompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyParents_CompanyId",
                table: "CompanyParents",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyParents_ParentCompanyId",
                table: "CompanyParents",
                column: "ParentCompanyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyParents");
        }
    }
}
