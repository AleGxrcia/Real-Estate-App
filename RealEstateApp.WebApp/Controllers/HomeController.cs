using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.ViewModels.Property;
using RealEstateApp.WebApp.Models;
using System.Diagnostics;

namespace RealEstateApp.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPropertyService _propertyService;
        private readonly IPropertyTypeService _propertyTypeService;
        private readonly IUserService _userService;
        public HomeController(IPropertyService propertyService, IPropertyTypeService propertyTypeService, IUserService userService)
        {
            _propertyService = propertyService;
            _propertyTypeService = propertyTypeService;
            _userService = userService;
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


        public async Task<IActionResult> Details(int id) 
        {
            return View(await _propertyService.GetPropertyDetails(id));
        }

        /*
        public async Task<IActionResult> Agents() 
        {
            return View(await _userService.getAllAgents());
        }*/


    }
}
