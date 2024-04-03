using RealEstateApp.Core.Application.ViewModels.Improvement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.ViewModels.Property
{
    public class PropertyViewModel
    {
        public int? Id { get; set; }
        public string PropertyType { get; set; }
        public int Code { get; set; }
        public string SaleType { get; set; }
        public decimal Price { get; set; }
        public decimal LandSize { get; set; }
        public int NumberOfRooms { get; set; }
        public int NumberOfBathrooms { get; set; }
        public string AgentId { get; set; }
        public int? PropertyTypeId { get; set; }

        public ICollection<ImprovementViewModel> Improvements { get; set; }
        public List<string> ImagesUrl { get; set; }
    }
}
