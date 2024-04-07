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



        public async Task UpdateImprovementsAsync(List<int> improvementsId, int propertyId)
        {
            // Eliminar todas las mejoras existentes para esta propiedad
            var existingImprovements = _dbContext.ImprovementProperties.Where(ip => ip.PropertyId == propertyId);
            _dbContext.ImprovementProperties.RemoveRange(existingImprovements);

            // Añadir las nuevas mejoras
            foreach (int improvementId in improvementsId)
            {
                var improvementProperty = new ImprovementProperty
                {
                    PropertyId = propertyId,
                    ImprovementId = improvementId
                };

                _dbContext.ImprovementProperties.Add(improvementProperty);
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateImagesAsync(List<string> photoUrls, int propertyId)
        {
            // Eliminar todas las imágenes existentes para esta propiedad
            var existingImages = _dbContext.PropertyImages.Where(pi => pi.PropertyId == propertyId);
            _dbContext.PropertyImages.RemoveRange(existingImages);

            // Añadir las nuevas imágenes
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


        public async Task DeleteImprovementPropertiesAsync(int propertyId)
        {
            // Obtener todas las ImprovementProperties asociadas a la propiedad
            var improvementProperties = _dbContext.ImprovementProperties.Where(ip => ip.PropertyId == propertyId);

            // Remover todas las ImprovementProperties asociadas
            _dbContext.ImprovementProperties.RemoveRange(improvementProperties);

            // Guardar los cambios en la base de datos
            await _dbContext.SaveChangesAsync();
        }


    }
}
