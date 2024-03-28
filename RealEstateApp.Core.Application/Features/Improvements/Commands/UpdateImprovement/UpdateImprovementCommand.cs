using AutoMapper;
using MediatR;
using RealEstateApp.Core.Application.Exceptions;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Wrappers;
using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Core.Application.Features.Improvements.Commands.UpdateImprovement;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using RealEstateApp.Core.Application.ViewModels.Improvement;

namespace RealEstateApp.Core.Application.Features.Improvements.Commands.UpdateImprovement
{
    /// <summary>
    /// Parámetros para la actualizacion de una improvement
    /// </summary> 
    public class UpdateImprovementCommand : IRequest<Response<ImprovementUpdateResponse>>
    {
        [SwaggerParameter(Description = "El id de la mejora que se quiere actualizar")]
        public int Id { get; set; }

        /// <example>Consolas</example>
        [SwaggerParameter(Description = "El nuevo nombre de la mejora")]
        public string Name { get; set; }

        /// <example>Dispositivos electronicos de entretenimiento</example>
        [SwaggerParameter(Description = "La nueva descripcion de la mejora")]
        public string? Description { get; set; }
    }
    public class UpdateImprovementCommandHandler : IRequestHandler<UpdateImprovementCommand, Response<ImprovementUpdateResponse>>
    {
        private readonly IImprovementRepository _improvementRepository;
        private readonly IMapper _mapper;

        public UpdateImprovementCommandHandler(IImprovementRepository improvementRepository, IMapper mapper)
        {
            _improvementRepository = improvementRepository;
            _mapper = mapper;
        }
        public async Task<Response<ImprovementUpdateResponse>> Handle(UpdateImprovementCommand command, CancellationToken cancellationToken)
        {
            var improvement = await _improvementRepository.GetByIdAsync(command.Id);

            if (improvement == null)
            {
                throw new ApiException($"Improvement not found.", (int)HttpStatusCode.NotFound);
            }
            else
            {
                improvement = _mapper.Map<Improvement>(command);
                await _improvementRepository.UpdateAsync(improvement, improvement.Id);
                var improvementVm = _mapper.Map<ImprovementUpdateResponse>(improvement);
                return new Response<ImprovementUpdateResponse>(improvementVm);
            }
        }
    }
}
