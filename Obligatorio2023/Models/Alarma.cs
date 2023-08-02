using System.ComponentModel.DataAnnotations.Schema;

namespace Obligatorio2023.Models
{
    public class Alarma
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string DatoEvaluar { get; set; }
        public float ValorLimite { get; set; }
        public Comparador Comparador { get; set; }
        public Dispositivo? Dispositivo { get; set; }

        [ForeignKey("Dispositivo")]
        public int DispositivoId { get; set; }

    }
}
