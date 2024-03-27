namespace RealEstateApp.Core.Domain.Entities
{
    public class ImprovementProperty
    {
        public int ImprovementId { get; set; }
        public int PropertyId { get; set; }
        public Property? Property { get; set; }
        public Improvement? Improvement { get; set; }
    }
}
