using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata.Ecma335;

namespace Obligatorio2023.Models
{
    public class RegistroAlarma
    {
        public int Id { get; set; }
        public DateTime FechaHoraGeneracion { get; set; }
        public string DatoEvaluar { get; set; }
        public float ValorLimite { get; set; }
        public string ValorRecibido { get; set; }
        public UPaciente? Paciente { get; set; }
        public Guid IdPaciente { get; set; }
        public Alarma? Alarma { get; set; }
        [ForeignKey("Alarma")]
        public int IdAlarma { get; set; }
    }
}
