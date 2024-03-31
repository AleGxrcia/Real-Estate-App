using AutoMapper;
using MediatR;
using RealEstateApp.Core.Application.Exceptions;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.ViewModels.Improvement;
using RealEstateApp.Core.Application.ViewModels.Property;
using RealEstateApp.Core.Application.Wrappers;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace RealEstateApp.Core.Application.Features.Agents.Queries.GetAgentProperties
{
    public class GetAgentPropertiesQuery : IRequest<Response<IList<PropertyViewModel>>>
    {
        [SwaggerParameter(Description = "El id del agente del que se desea seleccionar las propiedades")]
        public string Id { get; set; }
    }

    public class GetAgentPropertiesQueryHandler : IRequestHandler<GetAgentPropertiesQuery, Response<IList<PropertyViewModel>>>
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IImprovementRepository _improvementRepository;
        private readonly IMapper _mapper;

        public GetAgentPropertiesQueryHandler(IPropertyRepository propertyRepository, IImprovementRepository improvementRepository, IMapper mapper)
        {
            _improvementRepository = improvementRepository;
            _propertyRepository = propertyRepository;
            _mapper = mapper;
        }

        public async Task<Response<IList<PropertyViewModel>>> Handle(GetAgentPropertiesQuery request, CancellationToken cancellationToken)
        {
            var propertyList = await GetAgentPropertiesViewModel(request.Id);
            if (propertyList == null || propertyList.Count == 0) throw new Exception("Properties not found");

            return new Response<IList<PropertyViewModel>>(propertyList);
        }

        private async Task<List<PropertyViewModel>> GetAgentPropertiesViewModel(string Id)
        {
            var propertyList = await _propertyRepository.GetAllWithIncludeAsync(new List<string> { "PropertyType", "SaleType", "ImprovementProperties", "Images" });

            if (propertyList == null || propertyList.Count == 0) throw new ApiException($"Properties not found."
                , (int)HttpStatusCode.NotFound);

            var filteredProperties = propertyList.Where(property => property.AgentId == Id).ToList();

            if (filteredProperties.Count == 0)
                throw new ApiException($"No properties found for this Agent: {Id}.", (int)HttpStatusCode.NotFound);

            var improvementIds = propertyList
            .SelectMany(property => property.ImprovementProperties)
            .Select(pi => pi.ImprovementId)
            .ToList();

            var improvements = await _improvementRepository.GetAllByIdAsync(improvementIds);

            var listViewModels = filteredProperties
                .Select(property => new PropertyViewModel
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
                        Description = improvements.FirstOrDefault(i => i.Id == pi.ImprovementId)?.Description,
                        Name = improvements.FirstOrDefault(i => i.Id == pi.ImprovementId)?.Name,

                    })
                    .ToList(),
                    ImagesUrl = property.Images.Where(img => img.ImageUrl != null).Select(img => img.ImageUrl).ToList()
                }).ToList();


            return listViewModels;
        }
    }
}
