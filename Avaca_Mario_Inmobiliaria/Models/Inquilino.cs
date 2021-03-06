using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Avaca_Mario_Inmobiliaria.Models
{
    public class Inquilino
    {
        [Display(Name = "Código")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Este campo es Obligatorio."), RegularExpression("[0-9]{8,10}", ErrorMessage = "Solo numeros y hasta 10 digitos")]
        public string DNI { get; set; }

        [RegularExpression(@"^[a-zA-Z\s]{2,254}", ErrorMessage = "Solo letras o espacios"), Required(ErrorMessage = "Este campo es Obligatorio.")]
        public string Nombre { get; set; }

        [RegularExpression(@"^[a-zA-Z\s]{2,254}", ErrorMessage = "Solo letras o espacios"), Required(ErrorMessage = "Este campo es Obligatorio.")]
        public string Apellido { get; set; }

        [Display(Name = "Teléfono"), Required(ErrorMessage = "Este campo es Obligatorio."), Phone]
        public string Telefono { get; set; }

        [Display(Prompt = "ejemplo@correo.com"), EmailAddress]
         [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Dirección de Correo electrónico incorrecta.")]
        public string Email { get; set; }

        [Display(Name = "Lugar de Trabajo"), Required(ErrorMessage = "Este campo es Obligatorio.")]
        public string LugarTrabajo { get; set; }

        public bool Activo { get; set; }

    }
}
