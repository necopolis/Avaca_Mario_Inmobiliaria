using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Avaca_Mario_Inmobiliaria.Models
{
    public class Pago
    {
        [Display(Name = "Código")]
        public int Id { get; set; }

        [Display(Name = "Pago N°")]
        public int NumeroPago { get; set; }

        [Display(Name = "Fecha")]
        public DateTime? FechaPago { get; set; }

        public decimal Importe { get; set; }
        
        [Display(Name = "Contrato")]
        public int ContratoId { get; set; }

        // Como si fuera acceso directo a la otra tabla
        [ForeignKey(nameof(ContratoId))]
        public Contrato Contrato { get; set; }
    }
}
