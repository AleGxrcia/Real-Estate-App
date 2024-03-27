using RealEstateApp.Core.Domain.Common;

namespace RealEstateApp.Core.Domain.Entities
{
    public class PropertyType : AuditableBaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Property>? Properties { get; set; }
    }
}
