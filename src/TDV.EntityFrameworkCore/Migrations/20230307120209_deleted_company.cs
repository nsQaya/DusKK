using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TDV.Migrations
{
    public partial class deleted_company : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Companies_CompanyId",
                table: "Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_Funerals_Companies_SenderCompanyId",
                table: "Funerals");

            migrationBuilder.DropForeignKey(
                name: "FK_FuneralWorkOrderDetails_Companies_GiverCompanyId",
                table: "FuneralWorkOrderDetails");

            migrationBuilder.DropTable(
                name: "CompanyDetails");

            migrationBuilder.DropTable(
                name: "CompanyParents");

            migrationBuilder.DropTable(
                name: "ContactCompanies");

            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_UserDetails_UserId",
                table: "UserDetails");

            migrationBuilder.DropIndex(
                name: "IX_FuneralWorkOrderDetails_GiverCompanyId",
                table: "FuneralWorkOrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_Funerals_SenderCompanyId",
                table: "Funerals");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_CompanyId",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "GiverCompanyId",
                table: "FuneralWorkOrderDetails");

            migrationBuilder.DropColumn(
                name: "SenderCompanyId",
                table: "Funerals");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Contracts");

            migrationBuilder.CreateIndex(
                name: "IX_UserDetails_UserId",
                table: "UserDetails",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserDetails_UserId",
                table: "UserDetails");

            migrationBuilder.AddColumn<int>(
                name: "GiverCompanyId",
                table: "FuneralWorkOrderDetails",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SenderCompanyId",
                table: "Funerals",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Contracts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CityId = table.Column<int>(type: "int", nullable: false),
                    QuarterId = table.Column<int>(type: "int", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Fax = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsDoer = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Prefix = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RunningCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaxAdministration = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    TaxNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Website = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Companies_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Companies_Quarters_QuarterId",
                        column: x => x.QuarterId,
                        principalTable: "Quarters",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CompanyDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    OrganizationUnitId = table.Column<long>(type: "bigint", nullable: false),
                    RegionId = table.Column<int>(type: "int", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InvoicePrefix = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    NetsisReferanceNo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyDetails_AbpOrganizationUnits_OrganizationUnitId",
                        column: x => x.OrganizationUnitId,
                        principalTable: "AbpOrganizationUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompanyDetails_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompanyDetails_Regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompanyParents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    ParentCompanyId = table.Column<int>(type: "int", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyParents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyParents_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompanyParents_Companies_ParentCompanyId",
                        column: x => x.ParentCompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContactCompanies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: true),
                    ContactId = table.Column<int>(type: "int", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactCompanies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContactCompanies_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ContactCompanies_Contacts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contacts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: true),
                    Brand = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Capactiy = table.Column<int>(type: "int", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EndExaminationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndGuarantyDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndInsuranceDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    Plate = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    TrackNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Year = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vehicles_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserDetails_UserId",
                table: "UserDetails",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FuneralWorkOrderDetails_GiverCompanyId",
                table: "FuneralWorkOrderDetails",
                column: "GiverCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Funerals_SenderCompanyId",
                table: "Funerals",
                column: "SenderCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_CompanyId",
                table: "Contracts",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_CityId",
                table: "Companies",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_QuarterId",
                table: "Companies",
                column: "QuarterId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyDetails_CompanyId",
                table: "CompanyDetails",
                column: "CompanyId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompanyDetails_OrganizationUnitId",
                table: "CompanyDetails",
                column: "OrganizationUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyDetails_RegionId",
                table: "CompanyDetails",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyParents_CompanyId",
                table: "CompanyParents",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyParents_ParentCompanyId",
                table: "CompanyParents",
                column: "ParentCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactCompanies_CompanyId",
                table: "ContactCompanies",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactCompanies_ContactId",
                table: "ContactCompanies",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_CompanyId",
                table: "Vehicles",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_Companies_CompanyId",
                table: "Contracts",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Funerals_Companies_SenderCompanyId",
                table: "Funerals",
                column: "SenderCompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FuneralWorkOrderDetails_Companies_GiverCompanyId",
                table: "FuneralWorkOrderDetails",
                column: "GiverCompanyId",
                principalTable: "Companies",
                principalColumn: "Id");
        }
    }
}
