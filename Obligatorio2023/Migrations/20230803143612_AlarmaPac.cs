using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Obligatorio2023.Migrations
{
    public partial class AlarmaPac : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alarma_Dispositivo_DispositivoId",
                table: "Alarma");

            migrationBuilder.DropIndex(
                name: "IX_Alarma_DispositivoId",
                table: "Alarma");

            migrationBuilder.DropColumn(
                name: "DispositivoId",
                table: "Alarma");

            migrationBuilder.AddColumn<Guid>(
                name: "IdPaciente",
                table: "Alarma",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Alarma_IdPaciente",
                table: "Alarma",
                column: "IdPaciente");

            migrationBuilder.AddForeignKey(
                name: "FK_Alarma_UPaciente_IdPaciente",
                table: "Alarma",
                column: "IdPaciente",
                principalTable: "UPaciente",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alarma_UPaciente_IdPaciente",
                table: "Alarma");

            migrationBuilder.DropIndex(
                name: "IX_Alarma_IdPaciente",
                table: "Alarma");

            migrationBuilder.DropColumn(
                name: "IdPaciente",
                table: "Alarma");

            migrationBuilder.AddColumn<int>(
                name: "DispositivoId",
                table: "Alarma",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Alarma_DispositivoId",
                table: "Alarma",
                column: "DispositivoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Alarma_Dispositivo_DispositivoId",
                table: "Alarma",
                column: "DispositivoId",
                principalTable: "Dispositivo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
