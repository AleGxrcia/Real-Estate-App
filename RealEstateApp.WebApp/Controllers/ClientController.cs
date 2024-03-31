using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Dtos.Account;
using RealEstateApp.Core.Application.Helpers;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.ViewModels.Property;

namespace RealEstateApp.WebApp.Controllers
{
    public class ClientController : Controller
    {
        private readonly IPropertyService _propertyService;
        private readonly IPropertyTypeService _propertyTypeService;
        private readonly IUserService _userService;
        private readonly AuthenticationResponse user;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClientController(IPropertyService propertyService, IPropertyTypeService propertyTypeService, IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            _propertyService = propertyService;
            _propertyTypeService = propertyTypeService;
            _userService = userService;
            user = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
        }

        public async Task<IActionResult> Index(string SearchString)
        {
            var propiedades = await _propertyService.GetAllWithIncludeAsync();
            if (!string.IsNullOrEmpty(SearchString))
            {
                propiedades = propiedades.Where(p => p.Code.ToString().Contains(SearchString)).ToList();
            }
            return View(propiedades);
        }



        public async Task<IActionResult> IndexFilter(FiltersPropertyViewModel vm)
        {
            ViewBag.PropetyTypes = await _propertyTypeService.GetAllViewModel();
            return View("Index", await _propertyService.GetAllWithFilters(vm));
        }

        /*
        public async Task<IActionResult> FavoriteProperties() 
        {
            return View("Index", await _userService.GetFavoriteProperties(user.id);
        }
        */

        /*
        public async Task<IActionResult> LikePropertie(int id) 
        {
           await _userService.LikePropertie(id);
           return RedirectToRoute(new { controller = "Client", action = "Index" });
        }
        */


    }
}
