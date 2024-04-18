using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Dtos.Account;
using RealEstateApp.Core.Application.Helpers;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.ViewModels.Property;
using RealEstateApp.Core.Application.ViewModels.User;

namespace RealEstateApp.WebApp.Controllers
{
    [Authorize(Roles = "Agent")]
    public class AgentController : Controller
    {
        private readonly IUserService _userService;
        private readonly AuthenticationResponse user;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public AgentController(IMapper mapper, IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
            user = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
            _mapper = mapper;
        }


       
        public async Task<IActionResult> Profile()
        {
            return View(_mapper.Map<EditUserViewModel>(await _userService.getUserByIdAsync(user.Id)));
        }


        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel vm) 
        {
            if (!ModelState.IsValid)
            {
                return View("Profile",vm);
            }

            string Error = await _userService.UpdateUser(vm);
            if (Error != "") 
            {
                ViewBag.Error = Error;
                return View("Profile", vm);
            }
            
            return RedirectToRoute(new { controller = "Property", action = "Index" });
        }

    }
}
