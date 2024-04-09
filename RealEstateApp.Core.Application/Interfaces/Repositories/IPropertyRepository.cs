using RealEstateApp.Core.Domain.Entities;

namespace RealEstateApp.Core.Application.Interfaces.Repositories
{
	public interface IPropertyRepository : IGenericRepository<Property>
	{
		Task AddImagesAsync(List<string> photoUrls, int propertyId);
		Task AddImprovementToPropertyAsync(List<int> improvementsId, int propertyId);

		Task UpdateImagesAsync(List<string> photoUrls, int propertyId);

		Task UpdateImprovementsAsync(List<int> improvementsId, int propertyId);

		Task DeleteImprovementPropertiesAsync(int propertyId);

    }
}
