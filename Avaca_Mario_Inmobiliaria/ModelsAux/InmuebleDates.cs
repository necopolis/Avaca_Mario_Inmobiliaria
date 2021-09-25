using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Avaca_Mario_Inmobiliaria.ModelsAux
{
    public class InmuebleDates
    {
        [Required(ErrorMessage ="Este campo es obligatorio")]
        public DateTime Desde { get; set; }
        
        
        [Required(ErrorMessage = "Este campo es obligatorio")]

        public DateTime Hasta { get; set; }
    }
}
