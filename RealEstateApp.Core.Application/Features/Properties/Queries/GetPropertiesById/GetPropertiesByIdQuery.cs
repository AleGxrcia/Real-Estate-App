using AutoMapper;
using MediatR;
using RealEstateApp.Core.Application.Exceptions;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.ViewModels.Improvement;
using RealEstateApp.Core.Application.ViewModels.Property;
using RealEstateApp.Core.Application.Wrappers;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace RealEstateApp.Core.Application.Features.Properties.Queries.GetPropertyById
{
    /// <summary>
    /// Parámetros para por id los propertyos
    /// </summary>  
    public class GetPropertiesByIdQuery : IRequest<Response<PropertyViewModel>>
    {
        /// <example>1</example>
        [SwaggerParameter(Description = "Debe colocar el id de la propiedad que quiere obtener")]
        [Required]
        public int Id { get; set; }
    }

    public class GetPropertyByIdQueryHadler : IRequestHandler<GetPropertiesByIdQuery, Response<PropertyViewModel>>
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IMapper _mapper;

        public GetPropertyByIdQueryHadler(IPropertyRepository propertyRepository, IMapper mapper)
        {
            _propertyRepository = propertyRepository;
            _mapper = mapper;
        }
        public async Task<Response<PropertyViewModel>> Handle(GetPropertiesByIdQuery request, CancellationToken cancellationToken)
        {
            var property = await GetByIdViewModel(request.Id);
            if (property == null) throw new ApiException($"Property not found.", (int)HttpStatusCode.NotFound);
            return new Response<PropertyViewModel>(property);
        }

        private async Task<PropertyViewModel> GetByIdViewModel(int id)
        {
            var propertyList = await _propertyRepository.GetAllWithIncludeAsync(new List<string> { "PropertyType", "SaleType", "ImprovementProperties", "Images" });

            if (propertyList == null || propertyList.Count == 0) throw new ApiException($"Properties not found."
         , (int)HttpStatusCode.NotFound);


            var property = propertyList.FirstOrDefault(f => f.Id == id);

            if (property == null) throw new ApiException($"Property not found."
         , (int)HttpStatusCode.NotFound);

            PropertyViewModel propertyVm = new()
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
                    .Select(img => img.ImageUrl).FirstOrDefault()
            };

            return propertyVm;
        }
    }


}
