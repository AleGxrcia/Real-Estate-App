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
        public HomeController(IPropertyService propertyService, IPropertyTypeService propertyTypeService)
        {
            _propertyService = propertyService;
            _propertyTypeService = propertyTypeService;
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


    }
}
