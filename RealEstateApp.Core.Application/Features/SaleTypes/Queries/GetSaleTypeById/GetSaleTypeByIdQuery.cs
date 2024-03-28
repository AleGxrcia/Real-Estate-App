using AutoMapper;
using MediatR;
using RealEstateApp.Core.Application.Exceptions;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.ViewModels.PropertyType;
using RealEstateApp.Core.Application.ViewModels.SaleType;
using RealEstateApp.Core.Application.Wrappers;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.Features.SaleTypes.Queries.GetSaleTypeById
{
    /// <summary>
    /// Parámetros para obtener un tipo de venta por su id
    /// </summary>  
    public class GetSaleTypeByIdQuery : IRequest<Response<SaleTypeViewModel>>
    {
        [SwaggerParameter(Description = "El id del tipo de venta que se desea seleccionar")]
        public int Id { get; set; }
    }
    public class GetSaleTypeByIdQueryHandler : IRequestHandler<GetSaleTypeByIdQuery, Response<SaleTypeViewModel>>
    {
        private readonly ISaleTypeRepository _saleTypeRepository;
        private readonly IMapper _mapper;

        public GetSaleTypeByIdQueryHandler(ISaleTypeRepository saleTypeRepository, IMapper mapper)
        {
            _saleTypeRepository = saleTypeRepository;
            _mapper = mapper;
        }

        public async Task<Response<SaleTypeViewModel>> Handle(GetSaleTypeByIdQuery query, CancellationToken cancellationToken)
        {
            var saleType = await _saleTypeRepository.GetByIdAsync(query.Id);

            if (saleType == null) throw new ApiException($"SaleType not found.", (int)HttpStatusCode.NotFound);
            var saleTypeVm = _mapper.Map<SaleTypeViewModel>(saleType);

            return new Response<SaleTypeViewModel>(saleTypeVm);

        }
    }

}
