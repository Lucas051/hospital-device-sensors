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

        public UPaciente UPaciente { get; set; }

        [ForeignKey("UPaciente")]
        public int PacienteId { get; set; }

    }
}
