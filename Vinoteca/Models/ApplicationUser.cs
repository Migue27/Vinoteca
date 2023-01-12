using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Vinoteca.Utilities;

namespace Vinoteca.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required(ErrorMessage = "Debe introducir su nombre")]
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        [Display(Name = "Dirección")]
        [Required(ErrorMessage = "Introduzca su dirección")]
        public string Direccion { get; set; }
        
        public bool Bodeguero { get; set; }

        //public string? ProfilePicturePath { get; set; }

        //PROPIOS USUARIOS
        public List<Pedido> Pedidos { get; set; }

        //BODEGUEROS
        [Display(Name ="Nombre de la bodega")]
        [RequiredIf("Bodeguero",true)]
        //[Required(ErrorMessage ="Debe introducir el nombre de la bodega")]
        public string NombreBodega { get; set; }
        [Display(Name ="CIF")]
        [RequiredIf("Bodeguero", true)]
        //[Required(ErrorMessage ="Debe introducir el CIF de facturación")]
        public string Cif { get; set; }
        public List<Vino> Vinos { get; set; }






    }
}
