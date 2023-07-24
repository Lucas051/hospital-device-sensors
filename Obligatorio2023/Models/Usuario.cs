using System.ComponentModel.DataAnnotations.Schema;

namespace Obligatorio2023.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Contraseña { get; set; }
        public string Email { get; set; }
        public string NombreApellido { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }

        [ForeignKey("RolId")]
        public int RolId { get; set; } // Clave foránea que referencia a la tabla "Roles"
        public Roles Rol { get; set; } // Para acceder al objeto "Roles"
    }
}

