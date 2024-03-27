using AutoMapper;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.ViewModels.SaleType;
using RealEstateApp.Core.Domain.Entities;

namespace RealEstateApp.Core.Application.Services
{
    public class SaleTypeService : GenericService<SaveSaleTypeViewModel, SaleTypeViewModel, SaleType>, ISaleTypeService
    {
        private readonly ISaleTypeRepository _saleTypeRepository;
        private readonly IMapper _mapper;

        public SaleTypeService(ISaleTypeRepository saleTypeRepository, IMapper mapper) : base(saleTypeRepository, mapper)
        {
            _saleTypeRepository = saleTypeRepository;
            _mapper = mapper;
        }
    }
}
