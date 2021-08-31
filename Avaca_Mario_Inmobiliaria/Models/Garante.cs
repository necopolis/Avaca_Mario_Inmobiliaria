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
        [Required]
        public string DNI { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        [Display(Name ="Lugar de Trabajo"),Required]
        public string LugarTrabajo { get; set; }
        [Display(Prompt = "Monto del Recibo de Sueldo", Name ="Recibo de Sueldo")]
        public decimal Sueldo { get; set; }
    }
}
