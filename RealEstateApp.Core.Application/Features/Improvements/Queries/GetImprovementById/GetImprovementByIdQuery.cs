using AutoMapper;
using MediatR;
using RealEstateApp.Core.Application.Exceptions;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.ViewModels.Improvement;
using RealEstateApp.Core.Application.Wrappers;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace RealEstateApp.Core.Application.Features.Improvements.Queries.GetImprovementById
{
    /// <summary>
    /// Parámetros para obtener una mejora por su id
    /// </summary>  
    public class GetImprovementByIdQuery : IRequest<Response<ImprovementViewModel>>
    {
        [SwaggerParameter(Description = "El id de la mejora que se desea seleccionar")]
        public int Id { get; set; }
    }
    public class GetImprovementByIdQueryHandler : IRequestHandler<GetImprovementByIdQuery, Response<ImprovementViewModel>>
    {
        private readonly IImprovementRepository _improvementRepository;
        private readonly IMapper _mapper;

        public GetImprovementByIdQueryHandler(IImprovementRepository improvementRepository, IMapper mapper)
        {
            _improvementRepository = improvementRepository;
            _mapper = mapper;
        }

        public async Task<Response<ImprovementViewModel>> Handle(GetImprovementByIdQuery query, CancellationToken cancellationToken)
        {
            var improvement = await _improvementRepository.GetByIdAsync(query.Id);

            if (improvement == null) throw new ApiException($"Improvement not found.", (int)HttpStatusCode.NotFound);

            var improvementVm = _mapper.Map<ImprovementViewModel>(improvement);

            return new Response<ImprovementViewModel>(improvementVm);
        }
    }

}
