using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TDV.Migrations
{
    public partial class Added_FuneralTransportOrders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FuneralTranportOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StartKM = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OperationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OperationKM = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeliveryKM = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndKM = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ReceiverFullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReceiverKinshipDegree = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FuneralWorkOrderDetailId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_FuneralTranportOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FuneralTranportOrders_FuneralWorkOrderDetails_FuneralWorkOrderDetailId",
                        column: x => x.FuneralWorkOrderDetailId,
                        principalTable: "FuneralWorkOrderDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FuneralTranportOrders_FuneralWorkOrderDetailId",
                table: "FuneralTranportOrders",
                column: "FuneralWorkOrderDetailId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FuneralTranportOrders");
        }
    }
}
