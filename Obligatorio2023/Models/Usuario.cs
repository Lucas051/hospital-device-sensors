﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Obligatorio2023.Models
{
    public abstract class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string NombreUsuario { get; set; }
        public string Contraseña { get; set; }
        public string Email { get; set; }
        public string NombreApellido { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
    }
}

