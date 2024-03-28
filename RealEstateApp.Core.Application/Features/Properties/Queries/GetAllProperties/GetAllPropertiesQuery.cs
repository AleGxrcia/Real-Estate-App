using AutoMapper;
using MediatR;
using RealEstateApp.Core.Application.Exceptions;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.ViewModels.Improvement;
using RealEstateApp.Core.Application.ViewModels.Property;
using RealEstateApp.Core.Application.Wrappers;
using System.Net;

namespace RealEstateApp.Core.Application.Features.Properties.Queries.GetAllProperties
{
    public class GetAllPropertiesQuery : IRequest<Response<IList<PropertyViewModel>>>
    {
    }

    public class GetAllPropertiesQueryHandler : IRequestHandler<GetAllPropertiesQuery, Response<IList<PropertyViewModel>>>
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IMapper _mapper;

        public GetAllPropertiesQueryHandler(IPropertyRepository propertyRepository, IMapper mapper)
        {
            _propertyRepository = propertyRepository;
            _mapper = mapper;
        }

        public async Task<Response<IList<PropertyViewModel>>> Handle(GetAllPropertiesQuery request, CancellationToken cancellationToken)
        {
            var propertyList = await GetAllViewModelWithFilters();
            if (propertyList == null || propertyList.Count == 0) throw new Exception("Properties not found");

            return new Response<IList<PropertyViewModel>>(propertyList);
        }

        private async Task<List<PropertyViewModel>> GetAllViewModelWithFilters()
        {
            var propertyList = await _propertyRepository.GetAllWithIncludeAsync(new List<string> { "PropertyType", "SaleType", "ImprovementProperties", "Images" });

            if (propertyList == null || propertyList.Count == 0) throw new ApiException($"Properties not found."
                , (int)HttpStatusCode.NotFound);

            var listViewModels = propertyList.Select(property => new PropertyViewModel
            {
                Id = property.Id,
                PropertyType = property.PropertyType.Name,
                Price = property.Price,
                Code = property.Code,
                LandSize = property.LandSize,
                NumberOfBathrooms = property.NumberOfBathrooms,
                NumberOfRooms = property.NumberOfRooms,
                SaleType = property.SaleType.Name,
                Improvements = property.ImprovementProperties
                    .Where(pi => pi.Improvement != null)
                    .Select(pi => new ImprovementViewModel
                    {
                        Name = pi.Improvement.Name,
                    }).ToList(),
                ImagesUrl = property.Images.Where(img => img.ImageUrl != null)
                .Select(img => img.ImageUrl).ToList(),
            }).ToList();


            return listViewModels;
        }
    }
}
