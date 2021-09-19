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

        [Display(Name = "Codigo de Pago")]
        public int NumeroPago { get; set; }


        [DisplayFormat(DataFormatString = "{0:dd-MM-yyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha de pago"), Required(ErrorMessage ="Campo solicitado")]
        public DateTime? FechaPago { get; set; }

        [Display(Name ="Monto a Pagar"), Required(ErrorMessage = "Campo solicitado"), DataType(DataType.Currency)]
        public decimal Importe { get; set; }
        
        [Display(Name = "Contrato")]
        public int ContratoId { get; set; }

        // Como si fuera acceso directo a la otra tabla
        [ForeignKey(nameof(ContratoId))]
        public Contrato Contrato { get; set; }
    }
}
