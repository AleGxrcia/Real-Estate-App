using AutoMapper;
using MediatR;
using RealEstateApp.Core.Application.Exceptions;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.ViewModels.PropertyType;
using RealEstateApp.Core.Application.Wrappers;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace RealEstateApp.Core.Application.Features.PropertyTypes.Queries.GetPropertyTypeById
{
    /// <summary>
    /// Parámetros para obtener un tipo de propiedad por su id
    /// </summary>  
    public class GetPropertyTypeByIdQuery : IRequest<Response<PropertyTypeViewModel>>
    {
        [SwaggerParameter(Description = "El id de la categoria que se desea seleccionar")]
        public int Id { get; set; }
    }
    public class GetPropertyTypeByIdQueryHandler : IRequestHandler<GetPropertyTypeByIdQuery, Response<PropertyTypeViewModel>>
    {
        private readonly IPropertyTypeRepository _propertyTypeRepository;
        private readonly IMapper _mapper;

        public GetPropertyTypeByIdQueryHandler(IPropertyTypeRepository propertyTypeRepository, IMapper mapper)
        {
            _propertyTypeRepository = propertyTypeRepository;
            _mapper = mapper;
        }

        public async Task<Response<PropertyTypeViewModel>> Handle(GetPropertyTypeByIdQuery query, CancellationToken cancellationToken)
        {
            var propertyType = await _propertyTypeRepository.GetByIdAsync(query.Id);

            if (propertyType == null) throw new ApiException($"PropertyType not found.", (int)HttpStatusCode.NotFound);
            var propertyTypeVm = _mapper.Map<PropertyTypeViewModel>(propertyType);

            return new Response<PropertyTypeViewModel>(propertyTypeVm);
        }
    }

}
