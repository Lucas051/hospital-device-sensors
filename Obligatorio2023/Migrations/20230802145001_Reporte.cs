using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Obligatorio2023.Migrations
{
    public partial class Reporte : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PresionArterial",
                table: "DatoReporte",
                newName: "PresionSistolica");

            migrationBuilder.AddColumn<float>(
                name: "PresionDistolica",
                table: "DatoReporte",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PresionDistolica",
                table: "DatoReporte");

            migrationBuilder.RenameColumn(
                name: "PresionSistolica",
                table: "DatoReporte",
                newName: "PresionArterial");
        }
    }
}
