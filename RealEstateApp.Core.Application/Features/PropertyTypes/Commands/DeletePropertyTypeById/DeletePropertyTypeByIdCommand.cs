using MediatR;
using RealEstateApp.Core.Application.Exceptions;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Wrappers;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.Features.PropertyTypes.Commands.DeletePropertyTypeById
{
    /// <summary>
    /// Parámetros para la eliminacion de un tipo de propiedad
    /// </summary> 
    public class DeletePropertyTypeByIdCommand : IRequest<Response<int>>
    {
        [SwaggerParameter(Description = "El id de el tipo de propiedad que se desea eliminar")]
        public int Id { get; set; }
    }
    public class DeletePropertyTypeByIdCommandHandler : IRequestHandler<DeletePropertyTypeByIdCommand, Response<int>>
    {
        private readonly IPropertyTypeRepository _propertyTypeRepository;
        public DeletePropertyTypeByIdCommandHandler(IPropertyTypeRepository propertyTypeRepository)
        {
            _propertyTypeRepository = propertyTypeRepository;
        }
        public async Task<Response<int>> Handle(DeletePropertyTypeByIdCommand command, CancellationToken cancellationToken)
        {
            var propertyType = await _propertyTypeRepository.GetByIdAsync(command.Id);
            if (propertyType == null) throw new ApiException($"PropertyType not found.", (int)HttpStatusCode.NotFound);
            await _propertyTypeRepository.DeleteAsync(propertyType);
            return new Response<int>(propertyType.Id);
        }
    }
}
