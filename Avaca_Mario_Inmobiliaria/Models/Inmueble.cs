using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Avaca_Mario_Inmobiliaria.Models
{
    public enum enUso
    {
        Comercial = 1,
        Residencial = 2,
    }
    public enum enTipo
    {
        Local = 1,
        Deposito = 2,
        Casa = 3,
        Departamento = 4,
        Otros = 5,
    }
    public class Inmueble
    {
        [Display(Name = "Código")]
        public int Id { get; set; }
        [Required]
        public string Direccion { get; set; }

        public int Uso { get; set; }
        public string UsoNombre => Uso > 0 ? ((enUso)Uso).ToString() : "";

        [Required(ErrorMessage ="Este campo es Obligatorio")]
        public int Tipo { get; set; }

        [Display(Name ="Tipo")]
        public string TipoNombre => Tipo > 0 ? ((enTipo)Tipo).ToString() : "";

        [Display(Name = "Ambientes"), Required(ErrorMessage = "Este campo es Obligatorio.")]
        public int CantAmbiente { get; set; }

        [Required(ErrorMessage = "Ingrese un numero con decimal")]
        public decimal Precio { get; set; }
        
        public bool Activo { get; set; }
        
        [Display(Name = "Dueño")]
        public int PropietarioId { get; set; }
        
        // Como si fuera acceso directo a la otra tabla
        [ForeignKey(nameof(PropietarioId))]
        public Propietario Duenio { get; set; }


        

        public static IDictionary<int, string> ObtenerUsos()
        {
            SortedDictionary<int, string> usos = new SortedDictionary<int, string>();
            Type tipoEnumRol = typeof(enUso);
            foreach (var valor in Enum.GetValues(tipoEnumRol))
            {
                usos.Add((int)valor, Enum.GetName(tipoEnumRol, valor));
            }
            return usos;
        }

        

        public static IDictionary<int, string> ObtenerTipos()
        {
            SortedDictionary<int, string> tipos = new SortedDictionary<int, string>();
            Type tipoEnumRol = typeof(enTipo);
            foreach (var valor in Enum.GetValues(tipoEnumRol))
            {
                tipos.Add((int)valor, Enum.GetName(tipoEnumRol, valor));
            }
            return tipos;
        }


    }
}
