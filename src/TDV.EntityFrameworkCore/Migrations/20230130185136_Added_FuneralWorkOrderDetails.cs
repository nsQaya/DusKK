using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TDV.Migrations
{
    public partial class Added_FuneralWorkOrderDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "FuneralWorkOrders",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "FuneralWorkOrderDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OperationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GiverCompanyId = table.Column<int>(type: "int", nullable: true),
                    EmployeeContactId = table.Column<int>(type: "int", nullable: true),
                    WorkOrderId = table.Column<int>(type: "int", nullable: true),
                    FuneralId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_FuneralWorkOrderDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FuneralWorkOrderDetails_Companies_GiverCompanyId",
                        column: x => x.GiverCompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FuneralWorkOrderDetails_Contacts_EmployeeContactId",
                        column: x => x.EmployeeContactId,
                        principalTable: "Contacts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FuneralWorkOrderDetails_Funerals_FuneralId",
                        column: x => x.FuneralId,
                        principalTable: "Funerals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FuneralWorkOrderDetails_FuneralWorkOrders_WorkOrderId",
                        column: x => x.WorkOrderId,
                        principalTable: "FuneralWorkOrders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FuneralWorkOrderDetails_EmployeeContactId",
                table: "FuneralWorkOrderDetails",
                column: "EmployeeContactId");

            migrationBuilder.CreateIndex(
                name: "IX_FuneralWorkOrderDetails_FuneralId",
                table: "FuneralWorkOrderDetails",
                column: "FuneralId");

            migrationBuilder.CreateIndex(
                name: "IX_FuneralWorkOrderDetails_GiverCompanyId",
                table: "FuneralWorkOrderDetails",
                column: "GiverCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_FuneralWorkOrderDetails_WorkOrderId",
                table: "FuneralWorkOrderDetails",
                column: "WorkOrderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FuneralWorkOrderDetails");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "FuneralWorkOrders");
        }
    }
}
