using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TDV.Migrations
{
    public partial class Added_Funeral : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Funerals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransferNo = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    MemberNo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    TcNo = table.Column<long>(type: "bigint", nullable: false),
                    PassportNo = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    LadingNo = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Statu = table.Column<int>(type: "int", nullable: false),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    ContactId = table.Column<int>(type: "int", nullable: false),
                    SenderCompanyId = table.Column<int>(type: "int", nullable: false),
                    OrganizationUnitId = table.Column<long>(type: "bigint", nullable: true),
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
                    table.PrimaryKey("PK_Funerals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Funerals_AbpOrganizationUnits_OrganizationUnitId",
                        column: x => x.OrganizationUnitId,
                        principalTable: "AbpOrganizationUnits",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Funerals_Companies_SenderCompanyId",
                        column: x => x.SenderCompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Funerals_Contacts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Funerals_FuneralTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "FuneralTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Funerals_ContactId",
                table: "Funerals",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_Funerals_OrganizationUnitId",
                table: "Funerals",
                column: "OrganizationUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Funerals_SenderCompanyId",
                table: "Funerals",
                column: "SenderCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Funerals_TypeId",
                table: "Funerals",
                column: "TypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Funerals");
        }
    }
}
