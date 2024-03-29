using AutoMapper;
using MediatR;
using RealEstateApp.Core.Application.Dtos.Account;
using RealEstateApp.Core.Application.Exceptions;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.ViewModels.Improvement;
using RealEstateApp.Core.Application.Wrappers;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace RealEstateApp.Core.Application.Features.Agents.Queries.GetAgentById
{
    /// <summary>
    /// Parámetros para obtener una mejora por su id
    /// </summary>  
    public class GetAgentByIdQuery : IRequest<Response<UserResponse>>
    {
        [SwaggerParameter(Description = "El id del agente que se desea seleccionar")]
        public string Id { get; set; }
    }
    public class GetAgentByIdQueryHandler : IRequestHandler<GetAgentByIdQuery, Response<UserResponse>>
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public GetAgentByIdQueryHandler(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;

            _mapper = mapper;
        }

        public async Task<Response<UserResponse>> Handle(GetAgentByIdQuery query, CancellationToken cancellationToken)
        {
            UserRequest userRequest = new();
            userRequest.Id = query.Id;
            var agent = await _accountService.GetUserWithId(userRequest);

            if (agent == null || agent.Role != "Agent") throw new ApiException($"Agent not found.", (int)HttpStatusCode.NotFound);


            return new Response<UserResponse>(agent);
        }
    }

}
