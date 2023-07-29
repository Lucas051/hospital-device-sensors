using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Obligatorio2023.Models;

namespace Obligatorio2023.Data
{
    public class ObligatorioContext : DbContext
    {
        public ObligatorioContext (DbContextOptions<ObligatorioContext> options)
            : base(options)
        {
        }

        public DbSet<Obligatorio2023.Models.UAdministrador> UAdministrador { get; set; } = default!;

        public DbSet<Obligatorio2023.Models.UPaciente>? UPaciente { get; set; }

        public DbSet<Obligatorio2023.Models.UMedico>? UMedico { get; set; }

        public DbSet<Obligatorio2023.Models.Dispositivo>? Dispositivo { get; set; }
    }
}
