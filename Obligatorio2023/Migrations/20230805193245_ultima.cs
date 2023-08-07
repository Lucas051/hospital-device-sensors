using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Obligatorio2023.Migrations
{
    public partial class ultima : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LogEndpoint",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreEndpoint = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaInvocacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Duracion = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogEndpoint", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UAdministrador",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NombreUsuario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Contraseña = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NombreApellido = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Direccion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UAdministrador", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UMedico",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Matricula = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Especialidad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NombreUsuario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Contraseña = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NombreApellido = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Direccion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UMedico", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UPaciente",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FechaNac = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TipoSangre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Observaciones = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NombreUsuario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Contraseña = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NombreApellido = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Direccion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UPaciente", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Alarma",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DatoEvaluar = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ValorLimite = table.Column<float>(type: "real", nullable: false),
                    Comparador = table.Column<int>(type: "int", nullable: false),
                    IdPaciente = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alarma", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Alarma_UPaciente_IdPaciente",
                        column: x => x.IdPaciente,
                        principalTable: "UPaciente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Dispositivo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Detalle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaHoraAlta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaHoraUltimaModificacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    PacienteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MedicoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Token = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dispositivo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dispositivo_UPaciente_PacienteId",
                        column: x => x.PacienteId,
                        principalTable: "UPaciente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RegistroAlarma",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaHoraGeneracion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DatoEvaluar = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ValorLimite = table.Column<float>(type: "real", nullable: false),
                    ValorRecibido = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdDispositivo = table.Column<int>(type: "int", nullable: false),
                    PacienteId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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
                    table.ForeignKey(
                        name: "FK_RegistroAlarma_UPaciente_PacienteId",
                        column: x => x.PacienteId,
                        principalTable: "UPaciente",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DatoReporte",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PresionSistolica = table.Column<float>(type: "real", nullable: false),
                    PresionDistolica = table.Column<float>(type: "real", nullable: false),
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
                name: "IX_Alarma_IdPaciente",
                table: "Alarma",
                column: "IdPaciente");

            migrationBuilder.CreateIndex(
                name: "IX_DatoReporte_DispositivoId",
                table: "DatoReporte",
                column: "DispositivoId");

            migrationBuilder.CreateIndex(
                name: "IX_Dispositivo_PacienteId",
                table: "Dispositivo",
                column: "PacienteId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistroAlarma_IdAlarma",
                table: "RegistroAlarma",
                column: "IdAlarma");

            migrationBuilder.CreateIndex(
                name: "IX_RegistroAlarma_PacienteId",
                table: "RegistroAlarma",
                column: "PacienteId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DatoReporte");

            migrationBuilder.DropTable(
                name: "LogEndpoint");

            migrationBuilder.DropTable(
                name: "RegistroAlarma");

            migrationBuilder.DropTable(
                name: "UAdministrador");

            migrationBuilder.DropTable(
                name: "UMedico");

            migrationBuilder.DropTable(
                name: "Dispositivo");

            migrationBuilder.DropTable(
                name: "Alarma");

            migrationBuilder.DropTable(
                name: "UPaciente");
        }
    }
}
