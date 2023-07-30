using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Obligatorio2023.Migrations
{
    public partial class RestoCompsyDisp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UPacienteId",
                table: "Dispositivo");

            migrationBuilder.RenameColumn(
                name: "UsuarioId",
                table: "Dispositivo",
                newName: "PacienteId");

            migrationBuilder.CreateTable(
                name: "Alarma",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DatoEvaluar = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ValorLimite = table.Column<float>(type: "real", nullable: false),
                    Comparador = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DispositivoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alarma", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Alarma_Dispositivo_DispositivoId",
                        column: x => x.DispositivoId,
                        principalTable: "Dispositivo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DatoReporte",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PresionArterial = table.Column<float>(type: "real", nullable: false),
                    Temperatura = table.Column<float>(type: "real", nullable: false),
                    SaturacionOxigeno = table.Column<float>(type: "real", nullable: false),
                    Pulso = table.Column<int>(type: "int", nullable: false),
                    FechaHoraUltRegistro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DispositivoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DatoReporte", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DatoReporte_Dispositivo_DispositivoId",
                        column: x => x.DispositivoId,
                        principalTable: "Dispositivo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Dispositivo_PacienteId",
                table: "Dispositivo",
                column: "PacienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Alarma_DispositivoId",
                table: "Alarma",
                column: "DispositivoId");

            migrationBuilder.CreateIndex(
                name: "IX_DatoReporte_DispositivoId",
                table: "DatoReporte",
                column: "DispositivoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Dispositivo_UPaciente_PacienteId",
                table: "Dispositivo",
                column: "PacienteId",
                principalTable: "UPaciente",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dispositivo_UPaciente_PacienteId",
                table: "Dispositivo");

            migrationBuilder.DropTable(
                name: "Alarma");

            migrationBuilder.DropTable(
                name: "DatoReporte");

            migrationBuilder.DropIndex(
                name: "IX_Dispositivo_PacienteId",
                table: "Dispositivo");

            migrationBuilder.RenameColumn(
                name: "PacienteId",
                table: "Dispositivo",
                newName: "UsuarioId");

            migrationBuilder.AddColumn<int>(
                name: "UPacienteId",
                table: "Dispositivo",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
