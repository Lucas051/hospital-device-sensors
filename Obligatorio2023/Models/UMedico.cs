using System.ComponentModel.DataAnnotations.Schema;

namespace Obligatorio2023.Models
{
    public class UMedico : Usuario
    {
        /*no hacen falta xque el id lo hereda de usuario*/
        //[ForeignKey("UsuarioId")]
        //public int UsuarioId { get; set; }
        public string Matricula { get; set; }
        public string Especialidad { get; set; }

        /*ejemplo funcion abstracta*/
        //public override void AltaDispositivo()
        //{

        //}

    }
}
