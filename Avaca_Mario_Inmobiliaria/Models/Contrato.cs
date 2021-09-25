using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Avaca_Mario_Inmobiliaria.Models
{
    public class Contrato
    {
        [Display(Name = "Código")]
        public int Id { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MM-yyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha Inicio"),
        DataType(DataType.DateTime),
        Required(ErrorMessage = "Este campo es Obligatorio.")]
        public DateTime FechaInicio { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MM-yyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha Fin"), DataType(DataType.Date), Required(ErrorMessage = "Este campo es Obligatorio.")]
        public DateTime FechaFin { get; set; }
        
        [Required]
        [Display(Name = "Inquilino")]
        public int InquilinoId { get; set; }

        // Como si fuera acceso directo a la otra tabla
        [ForeignKey(nameof(InquilinoId))]
        public Inquilino Inquilino { get; set; }


        [Required]
        [Display(Name = "Garante")]
        public int GaranteId { get; set; }

        // Como si fuera acceso directo a la otra tabla
        [ForeignKey(nameof(GaranteId))]
        public Garante Garante { get; set; }

        [Required]
        [Display(Name = "Inmueble")]
        public int InmuebleId { get; set; }

        // Como si fuera acceso directo a la otra tabla
        [ForeignKey(nameof(InmuebleId))]
        public Inmueble Inmueble { get; set; }

        public bool Activo { get; set; }
    }
}
