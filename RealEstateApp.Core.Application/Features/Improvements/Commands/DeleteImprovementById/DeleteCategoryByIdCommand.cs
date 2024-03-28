using MediatR;
using RealEstateApp.Core.Application.Exceptions;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Wrappers;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace RealEstateApp.Core.Application.Features.Improvements.Commands.DeleteImprovementById
{
    /// <summary>
    /// Parámetros para la eliminacion de una mejora
    /// </summary> 
    public class DeleteImprovementByIdCommand : IRequest<Response<int>>
    {
        [SwaggerParameter(Description = "El id de la mejora que se desea eliminar")]
        public int Id { get; set; }
    }
    public class DeleteImprovementByIdCommandHandler : IRequestHandler<DeleteImprovementByIdCommand, Response<int>>
    {
        private readonly IImprovementRepository _improvementRepository;
        public DeleteImprovementByIdCommandHandler(IImprovementRepository improvementRepository)
        {
            _improvementRepository = improvementRepository;
        }
        public async Task<Response<int>> Handle(DeleteImprovementByIdCommand command, CancellationToken cancellationToken)
        {
            var improvement = await _improvementRepository.GetByIdAsync(command.Id);
            if (improvement == null) throw new ApiException($"Improvement not found.", (int)HttpStatusCode.NotFound);
            await _improvementRepository.DeleteAsync(improvement);
            return new Response<int>(improvement.Id);
        }
    }
}
