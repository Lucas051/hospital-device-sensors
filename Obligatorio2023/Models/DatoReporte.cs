using System.ComponentModel.DataAnnotations.Schema;

namespace Obligatorio2023.Models
{
    public class DatoReporte
    {
        public int Id { get; set; }
        public float PresionArterial { get; set; }
        public float Temperatura { get; set; }
        public float SaturacionOxigeno { get; set; }
        public int Pulso { get; set; }
        public DateTime FechaHoraUltRegistro { get; set; }

        public Dispositivo? Dispositivo { get; set; }

        [ForeignKey("Dispositivo")]
        public int DispositivoId { get; set; }
    }
}
