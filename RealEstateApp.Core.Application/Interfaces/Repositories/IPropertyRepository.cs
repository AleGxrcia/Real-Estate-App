using RealEstateApp.Core.Domain.Entities;

namespace RealEstateApp.Core.Application.Interfaces.Repositories
{
	public interface IPropertyRepository : IGenericRepository<Property>
	{
		Task AddImagesAsync(List<string> photoUrls, int propertyId);
	}
}
