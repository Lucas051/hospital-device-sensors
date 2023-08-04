using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Obligatorio2023.Migrations
{
    public partial class AlarmaRegistro : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RegistroAlarma",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaHoraGeneracion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ValorRecibido = table.Column<float>(type: "real", nullable: false),
                    IdPaciente = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdAlarma = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistroAlarma", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegistroAlarma_Alarma_IdAlarma",
                        column: x => x.IdAlarma,
                        principalTable: "Alarma",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RegistroAlarma_IdAlarma",
                table: "RegistroAlarma",
                column: "IdAlarma");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RegistroAlarma");
        }
    }
}
