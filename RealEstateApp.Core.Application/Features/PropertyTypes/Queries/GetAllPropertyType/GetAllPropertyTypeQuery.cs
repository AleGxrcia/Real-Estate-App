using AutoMapper;
using MediatR;
using RealEstateApp.Core.Application.Exceptions;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.ViewModels.PropertyType;
using RealEstateApp.Core.Application.Wrappers;
using System.Net;

namespace RealEstateApp.Core.Application.Features.PropertyTypes.Queries.GetAllSaleType
{
    /// <summary>
    /// Parámetros para el listado de tipos de propiedad
    /// </summary>  
    public class GetAllPropertyTypeQuery : IRequest<Response<IList<PropertyTypeViewModel>>>
    {
    }
    public class GetAllSaleTypeQueryHandler : IRequestHandler<GetAllPropertyTypeQuery, Response<IList<PropertyTypeViewModel>>>
    {
        private readonly IPropertyTypeRepository _propertyTypeRepository;
        private readonly IMapper _mapper;

        public GetAllSaleTypeQueryHandler(IPropertyTypeRepository propertyTypeRepository, IMapper mapper)
        {
            _propertyTypeRepository = propertyTypeRepository;
            _mapper = mapper;
        }

        public async Task<Response<IList<PropertyTypeViewModel>>> Handle(GetAllPropertyTypeQuery request, CancellationToken cancellationToken)
        {

            return await GetAllViewModelWithInclude();
        }

        private async Task<Response<IList<PropertyTypeViewModel>>> GetAllViewModelWithInclude()
        {
            var propertyTypeList = await _propertyTypeRepository.GetAllAsync();

            if (propertyTypeList == null || propertyTypeList.Count == 0) throw new ApiException($"Property Type not found.", (int)HttpStatusCode.NotFound);

            return new Response<IList<PropertyTypeViewModel>>(propertyTypeList.Select(propertyType => _mapper.Map<PropertyTypeViewModel>(propertyType)).ToList());
        }
    }
}
