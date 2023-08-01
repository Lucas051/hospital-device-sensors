using System.ComponentModel.DataAnnotations.Schema;

namespace Obligatorio2023.Models
{
    public class UPaciente : Usuario
    {
        public DateTime FechaNac { get; set; }
        public string TipoSangre { get; set; }
        public string Observaciones { get; set; }
    }
}
