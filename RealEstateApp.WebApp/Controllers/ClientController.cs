using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Dtos.Account;
using RealEstateApp.Core.Application.Helpers;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.Services;
using RealEstateApp.Core.Application.ViewModels.Property;
using RealEstateApp.Infrastructure.Identity.Services;

namespace RealEstateApp.WebApp.Controllers
{
	[Authorize(Roles = "Client")]
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
            _httpContextAccessor = httpContextAccessor;
            user = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
        }


        public async Task<IActionResult> Index(string SearchString)
        {
            var propiedades = await _propertyService.GetAllWithIncludeAsync();
            ViewBag.PropetyTypes = await _propertyTypeService.GetAllViewModel();
            ViewBag.FavoriteProperties = await _userService.GetFavoriteProperties(user.Id);

            if (!string.IsNullOrEmpty(SearchString))
            {
                propiedades = propiedades.Where(p => p.Code.ToString().Contains(SearchString)).ToList();
            }

            propiedades.Reverse(); 

            return View(propiedades);
        }

        public async Task<IActionResult> IndexFilter(FiltersPropertyViewModel vm)
        {
            ViewBag.PropetyTypes = await _propertyTypeService.GetAllViewModel();
            ViewBag.FavoriteProperties = await _userService.GetFavoriteProperties(user.Id);

            var filteredProperties = await _propertyService.GetAllWithFilters(vm);
            filteredProperties.Reverse(); 

            return View("Index", filteredProperties);
        }



        public async Task<IActionResult> LikeProperty(int id) 
        {
           await _userService.AddFavorite(user.Id, id);
           return RedirectToRoute(new { controller = "Client", action = "Index" });
        }

		public async Task<IActionResult> DisLikeProperty(int id)
		{
			await _userService.RemoveFavorite(user.Id, id);
			return RedirectToRoute(new { controller = "Client", action = "Index" });
		}


		public async Task<IActionResult> FavoriteProperties()
		{
            ViewBag.PropetyTypes = await _propertyTypeService.GetAllViewModel();
            ViewBag.FavoriteProperties = await _userService.GetFavoriteProperties(user.Id);

            var favoriteProperties = await _userService.GetFavoriteProperties(user.Id);
            var allProperties = await _propertyService.GetAllWithIncludeAsync();

            var favoritePropertiesVm = allProperties
            .Where(property => favoriteProperties.Any(fp => fp.Id == property.Id))
            .ToList();


            return View("Index", favoritePropertiesVm);
		}

	}
}
