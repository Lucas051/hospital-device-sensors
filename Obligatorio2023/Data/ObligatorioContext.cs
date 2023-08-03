using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Obligatorio2023.Models;

namespace Obligatorio2023.Data
{
    public class ObligatorioContext : DbContext
    {
        public ObligatorioContext()
        {
        }

        public ObligatorioContext (DbContextOptions<ObligatorioContext> options)
            : base(options)
        {
        }

        public DbSet<Obligatorio2023.Models.UAdministrador> UAdministrador { get; set; } = default!;

        public DbSet<Obligatorio2023.Models.UPaciente>? UPaciente { get; set; }

        public DbSet<Obligatorio2023.Models.UMedico>? UMedico { get; set; }

        public DbSet<Obligatorio2023.Models.Dispositivo>? Dispositivo { get; set; }

        public DbSet<Obligatorio2023.Models.Alarma>? Alarma { get; set; }

        public DbSet<Obligatorio2023.Models.DatoReporte>? DatoReporte { get; set; }
        public DbSet<Obligatorio2023.Models.LogEndpoint>? LogEndpoint { get; set; }


        public void LogInvocacionEndpoint(string NombreEndpoint, DateTime FechaInvocacion, int Duracion)
        {
            var logEndpoint = new LogEndpoint
            {
                NombreEndpoint = NombreEndpoint,
                FechaInvocacion = FechaInvocacion,
                Duracion = Duracion
            };

            this.Set<LogEndpoint>().Add(logEndpoint);
            this.SaveChanges();
        }
    }
}
