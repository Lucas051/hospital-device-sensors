﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Obligatorio2023.Models
{
    public class Alarma
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string DatoEvaluar { get; set; }
        public float ValorLimite { get; set; }
        public Comparador Comparador { get; set; }
        public UPaciente? Paciente { get; set; }
        [ForeignKey("Paciente")]
        public Guid IdPaciente { get; set; }


    }
}
