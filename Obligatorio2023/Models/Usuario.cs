﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Obligatorio2023.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string NombreUsuario { get; set; }
        public string Contraseña { get; set; }
        public string Email { get; set; }
        public string NombreApellido { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
    }
}

