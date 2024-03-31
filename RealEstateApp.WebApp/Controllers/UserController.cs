using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Dtos.Account;
using RealEstateApp.Core.Application.Enums;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.ViewModels.User;

namespace RealEstateApp.WebApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IPropertyService _propertyService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IPropertyService propertyService, IMapper mapper)
        {
            _userService = userService;
            _propertyService = propertyService;
            _mapper = mapper;
        }

        public async Task<IActionResult> AgentManagement()
        {
            var users = await _userService.GetAllUsers();
            var agents = users.Where(a => a.Role == Roles.Agent.ToString()).ToList();

            List<AgentViewModel> agentUsers = _mapper.Map<List<AgentViewModel>>(agents);

            foreach (var agent in agentUsers)
            {
                var properties = await _propertyService.GetAllPropertiesByAgentId(agent.Id);
                agent.PropertyCount = properties.Count;
            }

            return View("AgentManagement", agentUsers);
        }

        public async Task<IActionResult> ConfirmStatusChange(string id)
        {
            UserResponse user = await _userService.getUserByIdAsync(id);
            return View("ConfirmStatusChange", user);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeUserStatus(UserResponse vm)
        {
            bool activate = vm.IsActive ? false : true;

            var result = await _userService.ChangeUserStatus(vm.Id, activate);
            return RedirectToRoute(new { controller = "User", action = "AgentManagement" });
        }
    }
}
