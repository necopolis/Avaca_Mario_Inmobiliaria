using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Avaca_Mario_Inmobiliaria.Models
{
    public class Garante
    {
        [Display(Name = "Código")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Este campo es Obligatorio.")]
        public string DNI { get; set; }

        [RegularExpression(@"^[a-zA-Z\s]{2,254}", ErrorMessage = "Solo letras o espacios"), Display(Prompt = "Juan")]
        public string Nombre { get; set; }

        [RegularExpression(@"^[a-zA-Z\s]{2,254}", ErrorMessage = "Solo letras o espacios"), Display(Prompt = "Lopez")]
        public string Apellido { get; set; }

        [Display(Name = "Numero de telefono"), Required, Phone]
        public string Telefono { get; set; }

        [Display(Prompt = "juanito@correo.com"), EmailAddress]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*",
             ErrorMessage = "Dirección de Correo electrónico incorrecta.")]
        public string Email { get; set; }

        [Display(Name ="Lugar de Trabajo"),Required]
        public string LugarTrabajo { get; set; }

        [Display(Prompt = "Monto del Recibo de Sueldo", Name ="Recibo de Sueldo")]
        public decimal Sueldo { get; set; }

        public bool Activo { get; set; }
    }
}
