﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Vinoteca.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Usuario / Email")]
        public string User { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }
        [Display(Name = "¿Recordar cuenta?")]
        public bool RememberMe { get; set; }

    }
}
