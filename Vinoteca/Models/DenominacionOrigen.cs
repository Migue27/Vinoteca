using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Vinoteca.Models
{
    public class DenominacionOrigen
    {
        public int ID { get; set; }


        [Display(Name ="Denominación de origen")]
        [Required(ErrorMessage ="Introduzca el nombre de la denominación.")]
        [StringLength(100)]
        public string Denominacion { get; set; }

        public List<Vino> Vinos { get; set; }
    }
}
