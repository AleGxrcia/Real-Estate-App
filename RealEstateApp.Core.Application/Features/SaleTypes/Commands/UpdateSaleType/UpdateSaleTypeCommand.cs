using AutoMapper;
using MediatR;
using RealEstateApp.Core.Application.Exceptions;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Wrappers;
using RealEstateApp.Core.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.Features.SaleTypes.Commands.UpdateSaleType
{
    /// <summary>
    /// Parámetros para la actualizacion de un tipo de venta
    /// </summary> 
    public class UpdateSaleTypeCommand : IRequest<Response<SaleTypeUpdateResponse>>
    {
        [SwaggerParameter(Description = "El id del tipo de venta que se quiere actualizar")]
        public int Id { get; set; }

        /// <example>Alquiler, venta</example>
        [SwaggerParameter(Description = "El nuevo nombre del tipo de venta")]
        public string Name { get; set; }

        /// <example>ALquiler con pago mensual de propiedad</example>
        [SwaggerParameter(Description = "La nueva descripcion del tipo de venta")]
        public string? Description { get; set; }
    }
    public class UpdateSaleTypeCommandHandler : IRequestHandler<UpdateSaleTypeCommand, Response<SaleTypeUpdateResponse>>
    {
        private readonly ISaleTypeRepository _saleTypeRepository;
        private readonly IMapper _mapper;

        public UpdateSaleTypeCommandHandler(ISaleTypeRepository saleTypeRepository, IMapper mapper)
        {
            _saleTypeRepository = saleTypeRepository;
            _mapper = mapper;
        }
        public async Task<Response<SaleTypeUpdateResponse>> Handle(UpdateSaleTypeCommand command, CancellationToken cancellationToken)
        {
            var saleType = await _saleTypeRepository.GetByIdAsync(command.Id);

            if (saleType == null)
            {
                throw new ApiException($"Sale Type not found.", (int)HttpStatusCode.NotFound);
            }
            else
            {
                saleType = _mapper.Map<SaleType>(command);
                await _saleTypeRepository.UpdateAsync(saleType, saleType.Id);
                var saleTypeVm = _mapper.Map<SaleTypeUpdateResponse>(saleType);
                return new Response<SaleTypeUpdateResponse>(saleTypeVm);
            }
        }
    }
}
