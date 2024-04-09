using Microsoft.AspNetCore.Http;
using RealEstateApp.Core.Application.ViewModels.Improvement;
using RealEstateApp.Core.Application.ViewModels.PropertyType;
using RealEstateApp.Core.Application.ViewModels.SaleType;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.ViewModels.Property
{
    public class SavePropertyViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Debe elegir un tipo de propiedad.")]
        [Range(0.01, int.MaxValue)]
        public int PropertyTypeId { get; set; }

        public ICollection<PropertyTypeViewModel>? PropertyTypes { get; set; }

        [Required(ErrorMessage = "Debe elegir un tipo de venta.")]
        [Range(0.01, int.MaxValue)]
        public int SaleTypeId { get; set; }

        public ICollection<SaleTypeViewModel>? SalesTypes { get; set; }

        [Required(ErrorMessage = "El precio es requerido.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor que cero.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Debe colocar una descripcion")]
        [DataType(DataType.Text)]
        public string Description { get; set; }


        [Required(ErrorMessage = "El tamaño de la propiedad es requerido.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El tamaño de la propiedad debe ser mayor que cero.")]
        public decimal LandSize { get; set; }


        [Required(ErrorMessage = "La cantidad de habitaciones es requerido.")]
        [Range(0.01, int.MaxValue, ErrorMessage = "La cantidad de habitaciones debe ser mayor que cero.")]
        public int NumberOfRooms { get; set; }


        [Required(ErrorMessage = "La cantidad de habitaciones es requerido.")]
        [Range(0.01, int.MaxValue, ErrorMessage = "La cantidad de habitaciones debe ser mayor que cero.")]
        public int NumberOfBathrooms { get; set; }


        [Required(ErrorMessage = "Debe seleccionar al menos una mejora.")]
        //[Range(0.01, int.MaxValue)]
        public List<int> ImprovementsId { get; set; }
        public ICollection<ImprovementViewModel>? Improvements { get; set; }

        public int? Code { get; set; }
        public string? AgentId { get; set; }

        //Imgagenes
        //[Required(ErrorMessage = "Debe colocar al menos la imagen de portada de la propiedad")]
        [DataType(DataType.Upload)]
        public IFormFile? file1 { get; set; }
        public string? ImgUrl1 { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile? file2 { get; set; }
        public string? ImgUrl2 { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile? file3 { get; set; }
        public string? ImgUrl3 { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile? file4 { get; set; }
        public string? ImgUrl4 { get; set; }
    }
}
