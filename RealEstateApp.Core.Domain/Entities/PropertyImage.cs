using RealEstateApp.Core.Domain.Common;

namespace RealEstateApp.Core.Domain.Entities
{
    public class PropertyImage : AuditableBaseEntity
    {
        public string ImageUrl { get; set; }
        public int PropertyId { get; set; }
    }
}
