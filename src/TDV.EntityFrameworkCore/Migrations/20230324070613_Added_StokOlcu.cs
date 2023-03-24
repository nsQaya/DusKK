using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TDV.Migrations
{
    public partial class Added_StokOlcu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StokOlcus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Alt = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Ust = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Deger = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StokId = table.Column<int>(type: "int", nullable: false),
                    OlcumId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StokOlcus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StokOlcus_Olcums_OlcumId",
                        column: x => x.OlcumId,
                        principalTable: "Olcums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StokOlcus_Stoks_StokId",
                        column: x => x.StokId,
                        principalTable: "Stoks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StokOlcus_OlcumId",
                table: "StokOlcus",
                column: "OlcumId");

            migrationBuilder.CreateIndex(
                name: "IX_StokOlcus_StokId",
                table: "StokOlcus",
                column: "StokId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StokOlcus");
        }
    }
}
