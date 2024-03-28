using AutoMapper;
using MediatR;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Wrappers;
using RealEstateApp.Core.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.Features.SaleTypes.Commands.Create

{
    /// <summary>
    /// Parámetros para la creacion de un tipo de venta
    /// </summary>  
    public class CreateSaleTypeCommand : IRequest<Response<int>>
    {
        /// <example> Alquiler, venta</example>
        [SwaggerParameter(Description = "El tipo de venta")]
        public string Name { get; set; }

        /// <example>Alquiler de hogar</example>
        [SwaggerParameter(Description = "La descripcion del tipo de venta")]
        public string? Description { get; set; }
    }
    public class CreateSaleTypeCommandHandler : IRequestHandler<CreateSaleTypeCommand, Response<int>>
    {
        private readonly ISaleTypeRepository _saleTypeRepository;
        private readonly IMapper _mapper;
        public CreateSaleTypeCommandHandler(ISaleTypeRepository saleTypeRepository, IMapper mapper)
        {
            _saleTypeRepository = saleTypeRepository;
            _mapper = mapper;
        }

        public async Task<Response<int>> Handle(CreateSaleTypeCommand request, CancellationToken cancellationToken)
        {
            var saleType = _mapper.Map<SaleType>(request);
            await _saleTypeRepository.AddAsync(saleType);
            return new Response<int>(saleType.Id);
        }
    }
}
