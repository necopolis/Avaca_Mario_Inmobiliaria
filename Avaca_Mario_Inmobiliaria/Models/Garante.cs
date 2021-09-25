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

        [Required(ErrorMessage = "Este campo es Obligatorio."), RegularExpression("[0-9]{8,10}", ErrorMessage = "Solo numeros y hasta 10 digitos")]
        public string DNI { get; set; }

        [RegularExpression(@"^[a-zA-Z\s]{2,254}", ErrorMessage = "Solo letras o espacios")]
        public string Nombre { get; set; }

        [RegularExpression(@"^[a-zA-Z\s]{2,254}", ErrorMessage = "Solo letras o espacios")]
        public string Apellido { get; set; }

        [Display(Name = "Numero de telefono"), Required, Phone]
        public string Telefono { get; set; }

        [Display(Prompt = "juanito@correo.com"), EmailAddress]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*",
             ErrorMessage = "Dirección de Correo electrónico incorrecta.")]
        public string Email { get; set; }

        [Display(Name ="Lugar de Trabajo"),Required]
        public string LugarTrabajo { get; set; }

        [Display(Name ="Recibo de Sueldo"), Required(ErrorMessage ="Este campo es Obligatorio")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "Debe ser mayor a 0")]
        public decimal Sueldo { get; set; }

        public bool Activo { get; set; }
    }
}
