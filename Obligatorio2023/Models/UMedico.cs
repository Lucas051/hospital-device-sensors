using System.ComponentModel.DataAnnotations.Schema;

namespace Obligatorio2023.Models
{
    public class UMedico : Usuario
    {
        public string Matricula { get; set; }
        public string Especialidad { get; set; }

    }
}
