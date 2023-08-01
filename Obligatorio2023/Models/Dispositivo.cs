using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Obligatorio2023.Models
{
    public class Dispositivo
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Detalle { get; set; }
        [Display(Name = "Fecha y Hora de Alta")]

        public DateTime FechaHoraAlta { get; set; }
        [Display(Name = "Última Modificación")]
      
        public DateTime FechaHoraUltimaModificacion { get; set; }
      
        public bool Activo { get; set; }
      
        public UPaciente? UPaciente { get; set; }

        [ForeignKey("UPaciente")]
        public Guid PacienteId { get; set; }

        [ForeignKey("UMedico")]
        public Guid MedicoId { get; set; }
    }
}
