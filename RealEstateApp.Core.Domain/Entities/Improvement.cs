using RealEstateApp.Core.Domain.Common;

namespace RealEstateApp.Core.Domain.Entities
{
    public class Improvement : AuditableBaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<ImprovementProperty>? ImprovementProperties { get; set; }
    }
}
