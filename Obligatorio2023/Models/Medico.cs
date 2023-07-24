using System.ComponentModel.DataAnnotations.Schema;

namespace Obligatorio2023.Models
{
    public class Medico : Usuario
    {
        [ForeignKey("UsuarioId")]
        public int UsuarioId { get; set; }
        public string Matricula { get; set; }
        public string Especialidad { get; set; }
    }
}
