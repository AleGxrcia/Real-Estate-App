using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Infrastructure.Persistence.Contexts;

namespace RealEstateApp.Infrastructure.Persistence.Repository
{
    public class PropertyRepository : GenericRepository<Property>, IPropertyRepository
    {
        private readonly ApplicationContext _dbContext;

        public PropertyRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }



        public async Task AddImagesAsync(List<string> photoUrls, int propertyId)
        {
            foreach (string photoUrl in photoUrls)
            {
                var propertyImage = new PropertyImage
                {
                    PropertyId = propertyId,
                    ImageUrl = photoUrl
                };

                _dbContext.PropertyImages.Add(propertyImage);
            }
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddImprovementToPropertyAsync(List<int> improvementsId, int propertyId)
        {
            foreach (int improvement in improvementsId)
            {
                var improventProperty = new ImprovementProperty
                {
                    PropertyId = propertyId,
                    ImprovementId = improvement
                };

                _dbContext.ImprovementProperties.Add(improventProperty);
            }
            await _dbContext.SaveChangesAsync();
        }
    }
}
