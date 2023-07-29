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
        public UPaciente Paciente { get; set; }

        public Usuario Creador { get; set; }

        [ForeignKey("PacienteId")]
        public int PacienteId { get; set; }
        [ForeignKey("CreadorId")]
        public int IdCreador { get; set; }
    }
}
