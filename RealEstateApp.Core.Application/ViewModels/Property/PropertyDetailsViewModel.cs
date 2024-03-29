using RealEstateApp.Core.Application.ViewModels.Improvement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.ViewModels.Property
{
    public class PropertyDetailsViewModel
    {
        public int? Id { get; set; }
        public string PropertyType { get; set; }
        public int Code { get; set; }
        public string SalesType { get; set; }
        public decimal Price { get; set; }
        public decimal LandSize { get; set; }
        public int NumberOfRooms { get; set; }
        public int NumberOfBathrooms { get; set; }
        public string Description { get; set; }
        public ICollection<ImprovementViewModel>? ImproveMentes { get; set; }

        public string AgentName { get; set; }
        public string AgentPhoneNumber { get; set; }
        public string AgentImgUrl { get; set; }
        public string AgentEmail { get; set; }

        public string? ImgUrl1 { get; set; }
        public string? ImgUrl2 { get; set; }
        public string? ImgUrl3 { get; set; }
        public string? ImgUrl4 { get; set; }
    }
}
