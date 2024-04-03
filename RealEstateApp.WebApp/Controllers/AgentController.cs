using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Dtos.Account;
using RealEstateApp.Core.Application.Helpers;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.ViewModels.Property;

namespace RealEstateApp.WebApp.Controllers
{
    public class AgentController : Controller
    {
        private readonly IPropertyService _propertyService;
        private readonly IPropertyTypeService _propertyTypeService;
        private readonly IUserService _userService;
        private readonly AuthenticationResponse user;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AgentController(IPropertyService propertyService, IPropertyTypeService propertyTypeService, IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            _propertyService = propertyService;
            _propertyTypeService = propertyTypeService;
            _userService = userService;
            user = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
        }


       
        public async Task<IActionResult> Profile()
        {
            return View(await _userService.getUserByIdAsync(user.Id));
        }


    }
}
