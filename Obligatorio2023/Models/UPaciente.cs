using System.ComponentModel.DataAnnotations.Schema;

namespace Obligatorio2023.Models
{
    public class UPaciente : Usuario
    {
        /*no hacen falta xque el id lo hereda de usuario*/
        //[ForeignKey("UsuarioId")]
        //public int UsuarioId { get; set; }
        public DateTime FechaNac { get; set; }
        public string TipoSangre { get; set; }
        public string Observaciones { get; set; }




        /*ejemplo constructor*/
        //public UPaciente(int usuarioId, DateTime fechaNac, string tipoSangre, string observaciones)
        //{
        //    UsuarioId = usuarioId;
        //    FechaNac = fechaNac;
        //    TipoSangre = tipoSangre;
        //    Observaciones = observaciones;
        //}
    }
}
