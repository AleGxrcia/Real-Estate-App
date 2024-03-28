using AutoMapper;
using MediatR;
using RealEstateApp.Core.Application.Exceptions;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.ViewModels.SaleType;
using RealEstateApp.Core.Application.Wrappers;
using System.Net;

namespace RealEstateApp.Core.Application.Features.SaleTypes.Queries.GetAllSaleType
{
    /// <summary>
    /// Parámetros para el listado de tipos de ventas
    /// </summary>  
    public class GetAllSaleTypeQuery : IRequest<Response<IList<SaleTypeViewModel>>>
    {
    }
    public class GetAllSaleTypeQueryHandler : IRequestHandler<GetAllSaleTypeQuery, Response<IList<SaleTypeViewModel>>>
    {
        private readonly ISaleTypeRepository _saleTypeRepository;
        private readonly IMapper _mapper;

        public GetAllSaleTypeQueryHandler(ISaleTypeRepository saleTypeRepository, IMapper mapper)
        {
            _saleTypeRepository = saleTypeRepository;
            _mapper = mapper;
        }

        public async Task<Response<IList<SaleTypeViewModel>>> Handle(GetAllSaleTypeQuery request, CancellationToken cancellationToken)
        {

            return await GetAllViewModel();
        }

        private async Task<Response<IList<SaleTypeViewModel>>> GetAllViewModel()
        {
            var saleTypeList = await _saleTypeRepository.GetAllAsync();

            if (saleTypeList == null || saleTypeList.Count == 0) throw new ApiException($"Sale Type not found.", (int)HttpStatusCode.NotFound);

            return new Response<IList<SaleTypeViewModel>>(saleTypeList.Select(saleType => _mapper.Map<SaleTypeViewModel>(saleType)).ToList());
        }
    }
}
