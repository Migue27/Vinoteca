using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Vinoteca.Models
{
    public enum EdadVino
    {
        [Display(Name ="Semi Crianza")]
        SemiCrianza,
        Crianza,
        Reserva,
        [Display(Name ="Gran Reserva")]
        GranReserva

    }

    public enum TipoVino
    {
        Tinto,
        Rosado,
        Blanco
    }

    public enum Roles
    {
        Admin,
        Bodeguero,
        Usuario
    }
}
