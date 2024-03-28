using AutoMapper;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.ViewModels.PropertyType;
using RealEstateApp.Core.Domain.Entities;
using System.Collections.Generic;

namespace RealEstateApp.Core.Application.Services
{
    public class PropertyTypeService : GenericService<SavePropertyTypeViewModel, PropertyTypeViewModel, PropertyType>, IPropertyTypeService
    {
        private readonly IPropertyTypeRepository _propertyTypeRepository;
        private readonly IMapper _mapper;

        public PropertyTypeService(IPropertyTypeRepository propertyTypeRepository, IMapper mapper) : base(propertyTypeRepository, mapper)
        {
            _propertyTypeRepository = propertyTypeRepository;
            _mapper = mapper;
        }

        public async Task<List<PropertyTypeViewModel>> GetAllViewModelWithInclude()
        {
            var listPropertyTypes = await _propertyTypeRepository.GetAllWithIncludeAsync(new List<string> { "Properties" });

            //var propertyCount = listPropertyTypes.Select(p => p.Properties).Count();

            return listPropertyTypes.Select(pt => new PropertyTypeViewModel()
            {
                Id = pt.Id,
                Name = pt.Name,
                Description = pt.Description,
                PropertyCount = pt.Properties.Count()
            }).ToList();
        }
    }
}
