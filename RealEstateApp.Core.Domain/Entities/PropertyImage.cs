using RealEstateApp.Core.Domain.Common;

namespace RealEstateApp.Core.Domain.Entities
{
    public class PropertyImage
    {
        public int id { get; set; }
        public string ImageUrl { get; set; }
        public int PropertyId { get; set; }
    }
}
