using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TDV.Migrations
{
    public partial class Removed_FuneralWorkOrder_FuneralWOrkOrderDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FuneralTranportOrders_FuneralWorkOrderDetails_FuneralWorkOrderDetailId",
                table: "FuneralTranportOrders");

            migrationBuilder.DropTable(
                name: "FuneralWorkOrderDetails");

            migrationBuilder.DropTable(
                name: "FuneralWorkOrders");

            migrationBuilder.DropIndex(
                name: "IX_FuneralTranportOrders_FuneralWorkOrderDetailId",
                table: "FuneralTranportOrders");

            migrationBuilder.DropColumn(
                name: "FuneralWorkOrderDetailId",
                table: "FuneralTranportOrders");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FuneralWorkOrderDetailId",
                table: "FuneralTranportOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "FuneralWorkOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    Statu = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FuneralWorkOrders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FuneralWorkOrderDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeContactId = table.Column<int>(type: "int", nullable: true),
                    FuneralId = table.Column<int>(type: "int", nullable: false),
                    WorkOrderId = table.Column<int>(type: "int", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    OperationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FuneralWorkOrderDetails", x => x.Id);
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
                name: "IX_FuneralTranportOrders_FuneralWorkOrderDetailId",
                table: "FuneralTranportOrders",
                column: "FuneralWorkOrderDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_FuneralWorkOrderDetails_EmployeeContactId",
                table: "FuneralWorkOrderDetails",
                column: "EmployeeContactId");

            migrationBuilder.CreateIndex(
                name: "IX_FuneralWorkOrderDetails_FuneralId",
                table: "FuneralWorkOrderDetails",
                column: "FuneralId");

            migrationBuilder.CreateIndex(
                name: "IX_FuneralWorkOrderDetails_WorkOrderId",
                table: "FuneralWorkOrderDetails",
                column: "WorkOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_FuneralTranportOrders_FuneralWorkOrderDetails_FuneralWorkOrderDetailId",
                table: "FuneralTranportOrders",
                column: "FuneralWorkOrderDetailId",
                principalTable: "FuneralWorkOrderDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
