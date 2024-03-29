using RealEstateApp.Core.Domain.Common;

namespace RealEstateApp.Core.Domain.Entities
{
    public class Property : AuditableBaseEntity
    {
        public int Code { get; set; }
        public decimal Price { get; set; }
        public decimal LandSize { get; set; }
        public int NumberOfRooms { get; set; }
        public int NumberOfBathrooms { get; set; }
        public string Description { get; set; }
        public int SaleTypeId { get; set; }
        public int PropertyTypeId { get; set; }
        public string AgentId { get; set; }
        
        //navegation properties
        public SaleType? SaleType { get; set; }
        public PropertyType? PropertyType { get; set; }
        public ICollection<PropertyImage>? Images { get; set; }
        public ICollection<FavoriteProperty>? FavoriteProperties { get; set; }
        public ICollection<ImprovementProperty>? ImprovementProperties { get; set; }
    }
}
