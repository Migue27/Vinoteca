using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Vinoteca.ViewModels
{
    public class NewBodegueroViewModel
    {
        [Display(Name = "Nombre de la bodega")]
        [Required(ErrorMessage ="Debe introducir el nombre de la bodega")]
        public string NombreBodega { get; set; }
        [Display(Name = "CIF")]
        [Required(ErrorMessage ="Debe introducir el CIF de facturación")]
        public string Cif { get; set; }
    }
}
