using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Vinoteca.Models;
using Microsoft.AspNetCore.Http;
using Vinoteca.Data;
using System.Collections;
using System.Collections.Generic;

namespace Vinoteca.ViewModels
{
    public class VinoViewModel: IEnumerable
    {
        private readonly ApplicationDbContext _context;

        private List<VinoViewModel> _vinos;

        public VinoViewModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public VinoViewModel()
        {

        }

        public IEnumerator GetEnumerator()
        {
            return _vinos.GetEnumerator();
        }
        public int ID { get; set; }

        [Required(ErrorMessage = "Debe introducir el nombre del vino")]
        public string Nombre { get; set; }
        [Display(Name = "Denominación de origen")]
        public DenominacionOrigen DenominacionOrigen { get; set; }
        public int? DenominacionOrigenId { get; set; }
        [Required(ErrorMessage = "Seleccione el tipo de vino")]
        [Display(Name = "Tipo de Vino")]
        [EnumDataType(typeof(TipoVino))]
        public TipoVino TipoVino { get; set; }
        [Display(Name = "Añada")]
        [Required(ErrorMessage = "Introduzca la añada del vino")]
        [Range(1800, 2023)]
        public int Anada { get; set; }
        [Display(Name = "Edad del vino")]
        [Required(ErrorMessage = "Seleccione la edad del vino")]
        [EnumDataType(typeof(EdadVino))]
        public EdadVino EdadVino { get; set; }
        [Required(ErrorMessage = "Introduzca la graduación del vino")]
        [RegularExpression(@"^[0-9]+.?\d{0,2}$", ErrorMessage = "La graduación debe tener solo 2 decimales")]
        [Range(0, 100, ErrorMessage = "El alcohol debe ser un número comprendido entre 0 y 100")]
        public float Alcohol { get; set; }
        [MaxLength(50, ErrorMessage = "El color no puede tener más de 50 caracteres")]
        public string Color { get; set; }
        [Display(Name = "Azucar residual (gr por litro)")]
        [Range(0, 99.99, ErrorMessage = "El valor debe estar comprendido entre 0 y 99,99 gr. por litro")]
        [RegularExpression(@"^[0-9]+.?\d{0,2}$", ErrorMessage = "Los gramos de azucar por litro deben tener solo 2 decimales")]
        public float? Azucar { get; set; }
        [Required(ErrorMessage = "Introduzca el número de botellas para vender")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Las unidades deben ser un número entero")]
        [Range(0, 10000000, ErrorMessage = "El número de botellas debe ser un valor positivo hasta 10.000.000")]
        public int Unidades { get; set; }
        [Required(ErrorMessage = "Introduzca el precio unitario")]
        [RegularExpression(@"^[0-9]+.?\d{0,2}$", ErrorMessage = "El precio debe tener solo 2 decimales")]
        [Range(0, 999999.99, ErrorMessage = "El precio debe ser un valor entre 0 y 1.000.000 €")]
        [Column(TypeName = "decimal(8,2)")]
        public decimal Precio { get; set; }
        [Display(Name = "Descripción")]
        [MaxLength(1000, ErrorMessage = "La descripción no puede tener más de 1000 caracteres")]
        public string Descripcion { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public string ApplicationUserId { get; set; }
        [Required(ErrorMessage ="La imagen es un campo obligatorio.")]
        public IFormFile VinoImage { get; set; }

        public string? VinoPath { get; set; }
    }
}
