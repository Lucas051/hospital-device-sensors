using System.ComponentModel.DataAnnotations.Schema;

namespace Obligatorio2023.Models
{
    public class Dispositivo
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Detalle { get; set; }
        public DateTime FechaHoraAlta { get; set; }
        public DateTime FechaHoraUltimaModificacion { get; set; }
        public bool Activo { get; set; }

        [ForeignKey("UPacienteId")]
        public int UPacienteId { get; set; }

        [ForeignKey("UsuarioId")]
        public int UsuarioId { get; set; }
    }
}
