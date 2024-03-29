using RealEstateApp.Core.Application.ViewModels.PropertyType;
using RealEstateApp.Core.Domain.Entities;

namespace RealEstateApp.Core.Application.Interfaces.Services
{
    public interface IPropertyTypeService : IGenericService<SavePropertyTypeViewModel, PropertyTypeViewModel, PropertyType>
    {
        Task<List<PropertyTypeViewModel>> GetAllViewModelWithInclude();
    }
}
