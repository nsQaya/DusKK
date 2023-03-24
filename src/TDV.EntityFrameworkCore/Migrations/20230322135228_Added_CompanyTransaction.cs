using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TDV.Migrations
{
    public partial class Added_CompanyTransaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CompanyTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InOut = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    No = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TaxRate = table.Column<int>(type: "int", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsTransferred = table.Column<bool>(type: "bit", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    FuneralId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    UnitType = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_CompanyTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyTransactions_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_CompanyTransactions_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_CompanyTransactions_DataLists_Type",
                        column: x => x.Type,
                        principalTable: "DataLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_CompanyTransactions_DataLists_UnitType",
                        column: x => x.UnitType,
                        principalTable: "DataLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_CompanyTransactions_Funerals_FuneralId",
                        column: x => x.FuneralId,
                        principalTable: "Funerals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyTransactions_CompanyId",
                table: "CompanyTransactions",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyTransactions_CurrencyId",
                table: "CompanyTransactions",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyTransactions_FuneralId",
                table: "CompanyTransactions",
                column: "FuneralId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyTransactions_Type",
                table: "CompanyTransactions",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyTransactions_UnitType",
                table: "CompanyTransactions",
                column: "UnitType");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyTransactions");
        }
    }
}
