using AutoMapper;
using MediatR;
using RealEstateApp.Core.Application.Dtos.Account;
using RealEstateApp.Core.Application.Exceptions;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.Wrappers;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.Features.Agents.Commands.ChangeAgentStatus
{
    public class ChangeAgentStatus
    { /// <summary>
      /// Parámetros para cambiar el estado de un agente por su id
      /// </summary>  
        public class ChangeAgentStatusCommand : IRequest<Response<UserResponse>>
        {
            [SwaggerParameter(Description = "El id del agente que se desea seleccionar")]
            public string Id { get; set; }
        }
        public class ChangeAgentStatusCommandHandler : IRequestHandler<ChangeAgentStatusCommand, Response<UserResponse>>
        {
            private readonly IAccountService _accountService;
            private readonly IMapper _mapper;

            public ChangeAgentStatusCommandHandler(IAccountService accountService, IMapper mapper)
            {
                _accountService = accountService;

                _mapper = mapper;
            }

            public async Task<Response<UserResponse>> Handle(ChangeAgentStatusCommand query, CancellationToken cancellationToken)
            {
                UserRequest userRequest = new();
                userRequest.Id = query.Id;
                var agent = await _accountService.GetUserWithId(userRequest);

                if (agent == null || agent.Role != "Agent") throw new ApiException($"Agent not found.", (int)HttpStatusCode.NotFound);


                return new Response<UserResponse>(agent);
            }
        }

    }
}
