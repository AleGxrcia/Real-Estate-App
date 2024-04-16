using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Dtos.Account;
using RealEstateApp.Core.Application.Enums;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.Helpers;
using RealEstateApp.Core.Application.ViewModels.User;
using Microsoft.AspNetCore.Authorization;

namespace RealEstateApp.WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IPropertyService _propertyService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse userViewModel;
        private readonly IMapper _mapper;

        public UserController(IHttpContextAccessor httpContextAccessor, IUserService userService, IPropertyService propertyService, IMapper mapper)
        {
            _userService = userService;
            _propertyService = propertyService;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            userViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
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

        public async Task<IActionResult> AdminManagement()
        {
            var users = await _userService.GetAllUsers();
            var adminsVm = users.Where(a => a.Role == Roles.Admin.ToString()).ToList();

            return View("AdminManagement", adminsVm);
        }

        public async Task<IActionResult> DevManagement()
        {
            var users = await _userService.GetAllUsers();
            var devsVm = users.Where(a => a.Role == Roles.Developer.ToString()).ToList();

            return View("DevManagement", devsVm);
        }

        public IActionResult CreateAdmin()
        {
            return View("SaveAdmin", new SaveUserAdminViewModel());
        }

        public IActionResult CreateDev()
        {
            return View("SaveDev", new SaveUserAdminViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> CreateAdmin(SaveUserAdminViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("SaveAdmin", vm);
            }

            var origin = Request.Headers["origin"];
            RegisterResponse response = await _userService.RegisterAdminAsync(vm, origin);
            if (response.HasError)
            {
                vm.HasError = response.HasError;
                vm.Error = response.Error;
                return View("SaveAdmin", vm);
            }

            return RedirectToRoute(new { controller = "User", action = "AdminManagement" });
        }

        [HttpPost]
        public async Task<IActionResult> CreateDev(SaveUserAdminViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("SaveDev", vm);
            }

            var origin = Request.Headers["origin"];
            RegisterResponse response = await _userService.RegisterDevAsync(vm, origin);
            if (response.HasError)
            {
                vm.HasError = response.HasError;
                vm.Error = response.Error;
                return View("SaveDev", vm);
            }

            return RedirectToRoute(new { controller = "User", action = "DevManagement" });
        }

        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userService.getUserByIdAsync(id);
            if (user.Id == userViewModel.Id)
            {
                return View("AdminManagement");
            }

            SaveUserAdminViewModel userVm = _mapper.Map<SaveUserAdminViewModel>(user);
            if (userVm.UserType == Roles.Admin.ToString())
            {
                return View("SaveAdmin", userVm);
            }
            else
            {
                return View("SaveDev", userVm);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SaveUserAdminViewModel vm)
        {
            bool isAdmin = vm.UserType == Roles.Admin.ToString() ? true : false;

            if (!ModelState.IsValid)
            {
                return View(isAdmin ? "SaveAdmin" : "SaveDev", vm);
            }

            UserResponse userVm = await _userService.getUserByIdAsync(vm.Id);
            await _userService.UpdateAsync(vm, vm.Id);

            return RedirectToRoute(new { controller = "User", action = isAdmin ? "AdminManagement" : "DevManagement" });
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

            if (vm.Role == Roles.Admin.ToString())
            {
                return RedirectToRoute(new { controller = "User", action = "AdminManagement" });
            }
            else if (vm.Role == Roles.Developer.ToString())
            {
                return RedirectToRoute(new { controller = "User", action = "DevManagement" });
            }
            else
            {
                return RedirectToRoute(new { controller = "User", action = "AgentManagement" });
            }
        }

        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userService.getUserByIdAsync(id);
            return View("Delete", user);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAgent(string id)
        {
            var agentProperties = await _propertyService.GetAllPropertiesByAgentId(id);
            if (agentProperties != null)
            {
                foreach (var property in agentProperties)
                {
                    await _propertyService.DeleteImprovementPropertiesAsync(property.Id.Value);
                    await _propertyService.Delete(property.Id.Value);

                    FileManagerHelper.DeleteFile(property.Id.Value, "PropertyImages");
                }
            }

            await _userService.DeleteUser(id);
            FileManagerHelper.DeleteFile(id, "UserProfile");

            return RedirectToRoute(new { controller = "User", action = "AgentManagement" });
        }

    }
}
