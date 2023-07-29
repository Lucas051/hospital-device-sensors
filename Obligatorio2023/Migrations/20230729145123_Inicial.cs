using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Obligatorio2023.Migrations
{
    public partial class Inicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dispositivo_Usuario_CreadorId",
                table: "Dispositivo");

            migrationBuilder.DropForeignKey(
                name: "FK_Dispositivo_Usuario_PacienteId",
                table: "Dispositivo");

            migrationBuilder.DropIndex(
                name: "IX_Dispositivo_CreadorId",
                table: "Dispositivo");

            migrationBuilder.DropIndex(
                name: "IX_Dispositivo_PacienteId",
                table: "Dispositivo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Usuario",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "CreadorId",
                table: "Dispositivo");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "Especialidad",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "Matricula",
                table: "Usuario");

            migrationBuilder.RenameTable(
                name: "Usuario",
                newName: "UPaciente");

            migrationBuilder.RenameColumn(
                name: "PacienteId",
                table: "Dispositivo",
                newName: "UsuarioId");

            migrationBuilder.RenameColumn(
                name: "IdCreador",
                table: "Dispositivo",
                newName: "UPacienteId");

            migrationBuilder.AlterColumn<string>(
                name: "TipoSangre",
                table: "UPaciente",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Observaciones",
                table: "UPaciente",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaNac",
                table: "UPaciente",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UPaciente",
                table: "UPaciente",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "UAdministrador",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UAdministrador");

            migrationBuilder.DropTable(
                name: "UMedico");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UPaciente",
                table: "UPaciente");

            migrationBuilder.RenameTable(
                name: "UPaciente",
                newName: "Usuario");

            migrationBuilder.RenameColumn(
                name: "UsuarioId",
                table: "Dispositivo",
                newName: "PacienteId");

            migrationBuilder.RenameColumn(
                name: "UPacienteId",
                table: "Dispositivo",
                newName: "IdCreador");

            migrationBuilder.AddColumn<int>(
                name: "CreadorId",
                table: "Dispositivo",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "TipoSangre",
                table: "Usuario",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Observaciones",
                table: "Usuario",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaNac",
                table: "Usuario",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Usuario",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Especialidad",
                table: "Usuario",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Matricula",
                table: "Usuario",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Usuario",
                table: "Usuario",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Dispositivo_CreadorId",
                table: "Dispositivo",
                column: "CreadorId");

            migrationBuilder.CreateIndex(
                name: "IX_Dispositivo_PacienteId",
                table: "Dispositivo",
                column: "PacienteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Dispositivo_Usuario_CreadorId",
                table: "Dispositivo",
                column: "CreadorId",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Dispositivo_Usuario_PacienteId",
                table: "Dispositivo",
                column: "PacienteId",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
