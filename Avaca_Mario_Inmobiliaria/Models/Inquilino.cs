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
        [Required]
        public string DNI { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        [Required]
        public string Telefono { get; set; }
        public string Email { get; set; }
        [Display(Name = "Lugar de Trabajo"),Required]
        public string LugarTrabajo { get; set; }

    }
}
