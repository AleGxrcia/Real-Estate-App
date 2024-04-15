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
            ViewBag.PropetyTypes = await _propertyTypeService.GetAllViewModel();

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
            var filteredProperties = await _propertyService.GetAllWithFilters(vm);
            filteredProperties.Reverse(); 
            return View("Index", filteredProperties);
        }


        public async Task<IActionResult> Agents(string SearchString)
        {
            var agents = await _userService.GetAllAgents();

            if (!string.IsNullOrEmpty(SearchString))
            {
                agents = agents.Where(a => a.FirstName.Contains(SearchString)).ToList();
            }

            agents = agents.OrderBy(a => a.FirstName).ToList(); // Ordenar por orden alfabético

            return View(agents);
        }


        public async Task<IActionResult> PropertiesByAgentId(string id) 
        {
            ViewBag.PropetyTypes = await _propertyTypeService.GetAllViewModel();
            var PropertiesByAgent = await _propertyService.GetAllPropertiesByAgentId(id);
            return View("Index", PropertiesByAgent);
        }

		public async Task<IActionResult> Details(int id)
		{
            
            return View(await _propertyService.GetPropertyDetails(id));
		}

	}
}
