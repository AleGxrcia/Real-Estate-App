using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Core.Application.ViewModels.Property;




namespace RealEstateApp.Core.Application.Interfaces.Services
{
	public interface IPropertyService : IGenericService<SavePropertyViewModel, PropertyViewModel, Property>
	{
		Task<List<PropertyViewModel>> GetAllWithIncludeAsync();
		Task<PropertyDetailsViewModel> GetPropertyDetails(int Id);
	}
}
