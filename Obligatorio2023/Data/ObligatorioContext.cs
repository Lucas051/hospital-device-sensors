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
        //por cada clase que persista en la bd se crea una property tipo generico
        public DbSet<Obligatorio2023.Models.Dispositivo> Dispositivo { get; set; } = default!;
        //por cada clase que persista en la bd se crea una property tipo generico
        public DbSet<Obligatorio2023.Models.Usuario>? Usuario { get; set; }
        //por cada clase que persista en la bd se crea una property tipo generico
        public DbSet<Obligatorio2023.Models.Roles>? Roles { get; set; }
        //por cada clase que persista en la bd se crea una property tipo generico
        public DbSet<Obligatorio2023.Models.Medico>? Medico { get; set; }
        //por cada clase que persista en la bd se crea una property tipo generico
        public DbSet<Obligatorio2023.Models.Administrador>? Administrador { get; set; }
        //por cada clase que persista en la bd se crea una property tipo generico
        public DbSet<Obligatorio2023.Models.Paciente>? Paciente { get; set; }
    }
}
