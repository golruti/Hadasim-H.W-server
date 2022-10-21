using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareServicesServer.Migrations
{
    public partial class addTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "MobilePhone",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "CoronaData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    PositiveDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RecoverDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoronaData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoronaData_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CoronaVaccineData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CoronaDataId = table.Column<int>(type: "int", nullable: false),
                    DateReceiptVaccination = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VaccineManufacturer = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoronaVaccineData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoronaVaccineData_CoronaData_CoronaDataId",
                        column: x => x.CoronaDataId,
                        principalTable: "CoronaData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CoronaData_ClientId",
                table: "CoronaData",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_CoronaVaccineData_CoronaDataId",
                table: "CoronaVaccineData",
                column: "CoronaDataId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CoronaVaccineData");

            migrationBuilder.DropTable(
                name: "CoronaData");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MobilePhone",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
