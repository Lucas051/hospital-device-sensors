using Microsoft.AspNetCore.Authentication;
using System.ComponentModel.DataAnnotations.Schema;

namespace Obligatorio2023.Models
{
    public class UAdministrador : Usuario
    {
        /*no hacen falta xque el id lo hereda de usuario*/
        //[ForeignKey("UsuarioId")]
        //public int UsuarioId { get; set; }

        /*ejemplo funcion abstracta*/
        //public override void AltaDispositivo()
        //{

        //}
    }
}
