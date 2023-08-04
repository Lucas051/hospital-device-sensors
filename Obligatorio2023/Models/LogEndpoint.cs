using System.Diagnostics;
using System.Numerics;

namespace Obligatorio2023.Models
{
    public class LogEndpoint
    {
        //NombreEndpoint, FechaInvocacion, Duracion
        public int Id { get; set; }
        public string NombreEndpoint { get; set; }
        public DateTime FechaInvocacion { get; set; }
        public int Duracion { get; set; }
    }
}
