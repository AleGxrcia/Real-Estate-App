using AutoMapper;
using MediatR;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.ViewModels.Improvement;
using RealEstateApp.Core.Application.Wrappers;
using RealEstateApp.Core.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;

namespace RealEstateApp.Core.Application.Features.Improvements.Commands.CreateImprovement
{
    /// <summary>
    /// Parámetros para la creacion de una categoria
    /// </summary>  
    public class CreateImprovementCommand : IRequest<Response<int>>
    {
        /// <example>Techo Nuevo</example>
        [SwaggerParameter(Description = "El nombre de la mejora")]
        public string Name { get; set; }

        /// <example>Techo completamente pavimentado</example>
        [SwaggerParameter(Description = "La descripcion de la mejora")]
        public string? Description { get; set; }
    }
    public class CreateImprovementCommandHandler : IRequestHandler<CreateImprovementCommand, Response<int>>
    {
        private readonly IImprovementRepository _improvementRepository;
        private readonly IMapper _mapper;
        public CreateImprovementCommandHandler(IImprovementRepository improvementRepository, IMapper mapper)
        {
            _improvementRepository = improvementRepository;
            _mapper = mapper;
        }

        public async Task<Response<int>> Handle(CreateImprovementCommand request, CancellationToken cancellationToken)
        {
            var improvement = _mapper.Map<Improvement>(request);
            await _improvementRepository.AddAsync(improvement);
            return new Response<int>(improvement.Id);
        }
    }
}
