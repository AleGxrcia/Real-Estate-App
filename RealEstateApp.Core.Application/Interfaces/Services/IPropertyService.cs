using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Core.Application.ViewModels.Property;




namespace RealEstateApp.Core.Application.Interfaces.Services
{
	public interface IPropertyService : IGenericService<SavePropertyViewModel, PropertyViewModel, Property>
	{
		Task<List<PropertyViewModel>> GetAllWithIncludeAsync();
		Task<PropertyDetailsViewModel> GetPropertyDetails(int Id);
		Task<List<PropertyViewModel>> GetAllPropertiesByAgentId(string Id);
        Task AddImagesAsync(List<string> photoUrls, int propertyId);
        Task AddImprovementToPropertyAsync(List<int> improvementsId, int propertyId);
		public Task<List<PropertyViewModel>> GetAllWithFilters(FiltersPropertyViewModel filters);
        Task UpdateImagesAsync(List<string> photoUrls, int propertyId);
        Task UpdateImprovementsAsync(List<int> improvementsId, int propertyId);

        Task DeleteImprovementPropertiesAsync(int propertyId);
    }
}
