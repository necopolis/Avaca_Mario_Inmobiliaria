using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Avaca_Mario_Inmobiliaria.Models
{
    public class Inmueble
    {
        [Display(Name = "Código")]
        public int Id { get; set; }
        [Required]
        public string Direccion { get; set; }
        [Display(Prompt = "Comercial o Residencial")]
        public string Uso { get; set; }
        [Display(Prompt = "departamentos, locales, depósitos, oficinas individuales, etc.")]
        public string Tipo { get; set; }
        [Display(Name = "Cantidad de Ambientes"), Required]
        public int CantAmbiente { get; set; }
        public decimal Precio { get; set; }
        public bool Activo { get; set; }
        
        [Display(Name = "Dueño")]
        public int PropietarioId { get; set; }
        
        // Como si fuera acceso directo a la otra tabla
        [ForeignKey(nameof(PropietarioId))]
        public Propietario Duenio { get; set; }



    }
}
