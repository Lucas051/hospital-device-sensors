using System.ComponentModel.DataAnnotations.Schema;

namespace Obligatorio2023.Models
{
    public class Paciente : Usuario
    {
        [ForeignKey("UsuarioId")]
        public int UsuarioId { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string TipoSangre { get; set; }
        public string Observaciones { get; set; }
    
    }
}
