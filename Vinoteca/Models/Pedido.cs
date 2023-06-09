﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Vinoteca.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vinoteca.Models
{
    public class Pedido
    {
        public int ID { get; set; }
        

        public ApplicationUser Usuario { get; set; }

        public string ApplicationUserId { get; set; }

        public List<Vino> Vinos { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaCompra
        {
            get { return FechaCompra; }
            set { FechaCompra = DateTime.Now; }
        }
        
        [DisplayFormat(DataFormatString ="{0:T}", ApplyFormatInEditMode =true)]
        public DateTime HoraCompra
        {
            get { return HoraCompra; }
            set { HoraCompra = DateTime.Now; }
        }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Coste
        {
            get { return Coste; }
            set
            {
                foreach(var vino in Vinos)
                {
                    Coste += vino.Precio * vino.Unidades;
                }
            }
        }



    }
}
