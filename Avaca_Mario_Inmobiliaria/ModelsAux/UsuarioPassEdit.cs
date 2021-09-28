using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Avaca_Mario_Inmobiliaria.ModelsAux
{
    public class UsuarioPassEdit
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Este campo es Obligatorio"), DataType(DataType.Password), Display(Name = "Contraseña Anterior")]
        public string PassVieja { get; set; }

        [Required(ErrorMessage = "Este campo es Obligatorio"), DataType(DataType.Password), Display(Name ="Contraseña Nueva")]
        public string NuevaPass { get; set; }
    }
}
