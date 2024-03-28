using AutoMapper;
using MediatR;
using RealEstateApp.Core.Application.Exceptions;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Wrappers;
using RealEstateApp.Core.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace RealEstateApp.Core.Application.Features.PropertyTypes.Commands.UpdatePropertyType
{
    /// <summary>
    /// Parámetros para la actualizacion de una categoria
    /// </summary> 
    public class UpdatePropertyTypeCommand : IRequest<Response<PropertyTypeUpdateResponse>>
    {
        [SwaggerParameter(Description = "El id de el tipo de propiedad que se quiere actualizar")]
        public int Id { get; set; }

        /// <example>apartamento, casa</example>
        [SwaggerParameter(Description = "El nuevo nombre del tipo de propiedad")]
        public string Name { get; set; }

        /// <example>Casa</example>
        [SwaggerParameter(Description = "La nueva descripcion del tipo de propiedad")]
        public string? Description { get; set; }
    }
    public class UpdatePropertyTypeCommandHandler : IRequestHandler<UpdatePropertyTypeCommand, Response<PropertyTypeUpdateResponse>>
    {
        private readonly IPropertyTypeRepository _propertyTypeRepository;
        private readonly IMapper _mapper;

        public UpdatePropertyTypeCommandHandler(IPropertyTypeRepository propertyTypeRepository, IMapper mapper)
        {
            _propertyTypeRepository = propertyTypeRepository;
            _mapper = mapper;
        }
        public async Task<Response<PropertyTypeUpdateResponse>> Handle(UpdatePropertyTypeCommand command, CancellationToken cancellationToken)
        {
            var propertyType = await _propertyTypeRepository.GetByIdAsync(command.Id);

            if (propertyType == null)
            {
                throw new ApiException($"PropertyType not found.", (int)HttpStatusCode.NotFound);
            }
            else
            {
                propertyType = _mapper.Map<PropertyType>(command);
                await _propertyTypeRepository.UpdateAsync(propertyType, propertyType.Id);
                var propertyTypeVm = _mapper.Map<PropertyTypeUpdateResponse>(propertyType);
                return new Response<PropertyTypeUpdateResponse>(propertyTypeVm);
            }
        }
    }
}
