using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Dtos.Account;
using RealEstateApp.Core.Application.Enums;
using RealEstateApp.Core.Application.Interfaces.Services;

namespace RealEstateApp.WebApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> Agent()
        {
            var users = await _userService.GetAllUsers();
            var agents = users.Select(a => a.Role == Roles.Agent.ToString()).ToList();

            return View("Agent", agents);
        }

        public async Task<IActionResult> ConfirmUserStatus(string id)
        {
            UserResponse user = await _userService.getUserByIdAsync(id);
            return View("ConfirmUserStatus", user);
        }

        [HttpPost]
        public async Task<IActionResult> ActiveUser(UserResponse vm)
        {
            UserRequest userRequest = new()
            {
                Id = vm.Id,
            };

            var result = await _userService.ActiveUser(userRequest.Id);
            return RedirectToRoute(new { controller = "User", action = "Agent" });
        }

        [HttpPost]
        public async Task<IActionResult> InactiveUser(UserResponse vm)
        {
            UserRequest userRequest = new()
            {
                Id = vm.Id,
            };

            var result = await _userService.InactiveUser(userRequest.Id);
            return RedirectToRoute(new { controller = "User", action = "Agent" });
        }
    }
}
