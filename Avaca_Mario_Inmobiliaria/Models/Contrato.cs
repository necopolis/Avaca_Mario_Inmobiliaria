﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Avaca_Mario_Inmobiliaria.Models
{
    public class Contrato
    {
        public int Id { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }

        [Display(Name = "Inquilino")]
        public int InquilinoId { get; set; }

        // Como si fuera acceso directo a la otra tabla
        [ForeignKey(nameof(InquilinoId))]
        public Inquilino Inquilino { get; set; }


        [Display(Name = "Garante")]
        public int GaranteId { get; set; }

        // Como si fuera acceso directo a la otra tabla
        [ForeignKey(nameof(GaranteId))]
        public Garante Garante { get; set; }


        [Display(Name = "Inmueble")]
        public int InmuebleId { get; set; }

        // Como si fuera acceso directo a la otra tabla
        [ForeignKey(nameof(InmuebleId))]
        public Inmueble Inmueble { get; set; }
    }
}