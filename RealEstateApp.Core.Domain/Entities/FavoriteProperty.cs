using RealEstateApp.Core.Domain.Common;

namespace RealEstateApp.Core.Domain.Entities
{
    public class FavoriteProperty : AuditableBaseEntity
    {
        public string ClientId { get; set; }
        public int PropertyId { get; set; }
        public Property? Property { get; set; }
    }
}
