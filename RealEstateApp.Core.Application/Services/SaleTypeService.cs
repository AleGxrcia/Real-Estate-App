using AutoMapper;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.ViewModels.PropertyType;
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

        public async Task<List<SaleTypeViewModel>> GetAllViewModelWithInclude()
        {
            var listSaleTypes = await _saleTypeRepository.GetAllWithIncludeAsync(new List<string> { "Properties" });

            return listSaleTypes.Select(pt => new SaleTypeViewModel()
            {
                Id = pt.Id,
                Name = pt.Name,
                Description = pt.Description,
                PropertyCount = pt.Properties.Count()
            }).ToList();
        }
    }
}
