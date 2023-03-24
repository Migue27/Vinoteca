using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Vinoteca.Models;
using Vinoteca.Utilities;

namespace Vinoteca.ViewModels
{
    public class EditUserViewModel
    {

        public string Id { get; set; }

        [Required]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,})+)$", ErrorMessage = "Debe introducir un email válido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Debe introducir su nombre")]
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        [Display(Name = "Dirección")]
        [Required(ErrorMessage = "Introduzca su dirección")]
        public string Direccion { get; set; }
        [Display(Name = "Teléfono")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Imagen")]
        public IFormFile? ImageProfile { get; set; }

        [Display(Name = "Nombre de la bodega")]
        //[RequiredIf("Bodeguero", true)]
        //[Required(ErrorMessage = "Debe introducir el nombre de la bodega")]
        public string? NombreBodega { get; set; }
        [Display(Name = "CIF")]
        //[RequiredIf("Bodeguero", true)]
        //[Required(ErrorMessage = "Debe introducir el CIF de facturación")]
        public string? Cif { get; set; }

    }
}
