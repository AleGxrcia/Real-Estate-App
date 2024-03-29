using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using RealEstateApp.Core.Application.Exceptions;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.ViewModels.User;
using RealEstateApp.Core.Application.Wrappers;
using System.Net;

namespace RealEstateApp.Core.Application.Features.Agents.Queries.GetAllAgents
{
    /// <summary>
    /// Parámetros para el listado de mejoras
    /// </summary>  
    public class GetAllAgentsQuery : IRequest<Response<IList<UserViewModel>>>
    {
    }

    public class GetAllAgentQueryHandler : IRequestHandler<GetAllAgentsQuery, Response<IList<UserViewModel>>>
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public GetAllAgentQueryHandler(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }

        public async Task<Response<IList<UserViewModel>>> Handle(GetAllAgentsQuery request, CancellationToken cancellationToken)
        {

            return await GetAllViewModel();
        }

        private async Task<Response<IList<UserViewModel>>> GetAllViewModel()
        {
            var agentList = await _accountService.GetAllUsers();

            IList<UserViewModel> agentsvm = new List<UserViewModel>();

            if (agentList == null || agentList.Count == 0) throw new ApiException($"Agent not found.", (int)HttpStatusCode.NotFound);
            foreach (var agent in agentList)
            {
                if (agent.Role == "Agent")
                {
                    agentsvm.Add(_mapper.Map<UserViewModel>(agent));
                }
            }
            return new Response<IList<UserViewModel>>(agentsvm);


        }
    }
}
