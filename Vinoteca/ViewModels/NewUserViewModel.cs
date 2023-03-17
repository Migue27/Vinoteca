using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Vinoteca.Models;

namespace Vinoteca.ViewModels
{
    public class NewUserViewModel
    {
        [Required]
        [Display(Name ="Usuario")]
        public string User { get; set; }
        [Required]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,})+)$", ErrorMessage = "Debe introducir un email válido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Debe introducir su nombre")]
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        [Display(Name = "Dirección")]
        [Required(ErrorMessage = "Introduzca su dirección")]
        public string Direccion { get; set; }
        [Display(Name ="Teléfono")]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(100,ErrorMessage = "La contraseña debe de tener 6 caracteres como mínimo.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar contraseña")]
        [Compare("Password", ErrorMessage = "Las dos contraseñas introducidas no coinciden.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Imagen")]
        public IFormFile? ImageProfile { get; set; }
    }
}
