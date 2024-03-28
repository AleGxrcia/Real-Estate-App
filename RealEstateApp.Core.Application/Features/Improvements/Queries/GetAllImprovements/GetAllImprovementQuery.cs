using AutoMapper;
using MediatR;
using RealEstateApp.Core.Application.Exceptions;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.ViewModels.Improvement;
using RealEstateApp.Core.Application.Wrappers;
using System.Net;

namespace RealEstateApp.Core.Application.Features.Improvements.Queries.GetAllImprovements
{
    /// <summary>
    /// Parámetros para el listado de mejoras
    /// </summary>  
    public class GetAllImprovementQuery : IRequest<Response<IList<ImprovementViewModel>>>
    {
    }

    public class GetAllImprovementQueryHandler : IRequestHandler<GetAllImprovementQuery, Response<IList<ImprovementViewModel>>>
    {
        private readonly IImprovementRepository _improvementRepository;
        private readonly IMapper _mapper;

        public GetAllImprovementQueryHandler(IImprovementRepository improvementRepository, IMapper mapper)
        {
            _improvementRepository = improvementRepository;
            _mapper = mapper;
        }

        public async Task<Response<IList<ImprovementViewModel>>> Handle(GetAllImprovementQuery request, CancellationToken cancellationToken)
        {

            return await GetAllViewModel();
        }

        private async Task<Response<IList<ImprovementViewModel>>> GetAllViewModel()
        {
            var improvementList = await _improvementRepository.GetAllAsync();

            if (improvementList == null || improvementList.Count == 0) throw new ApiException($"Improvement not found.", (int)HttpStatusCode.NotFound);

            return new Response<IList<ImprovementViewModel>>(improvementList.Select(improvement => _mapper.Map<ImprovementViewModel>(improvement)).ToList());
        }
    }
}
