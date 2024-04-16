using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.ViewModels.PropertyType;

namespace RealEstateApp.WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PropertyTypeController : Controller
    {
        private readonly IPropertyTypeService _propertyTypeService;

        public PropertyTypeController(IPropertyTypeService propertyTypeService)
        {
            _propertyTypeService = propertyTypeService;
        }

        public async Task<IActionResult> Index()
        {
            var propertyTypes = await _propertyTypeService.GetAllViewModelWithInclude();
            return View(propertyTypes);
        }

        public IActionResult Create()
        {
            return View("SavePropertyType", new SavePropertyTypeViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(SavePropertyTypeViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("SavePropertyType", vm);
            }

            await _propertyTypeService.Add(vm);

            return RedirectToRoute(new { controller = "PropertyType", action = "Index" });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var propertyTypeVm = await _propertyTypeService.GetByIdSaveViewModel(id);
            return View("SavePropertyType", propertyTypeVm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SavePropertyTypeViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("SavePropertyType", vm);
            }

            await _propertyTypeService.Update(vm, vm.Id);

            return RedirectToRoute(new { controller = "PropertyType", action = "Index" });
        }

        public async Task<IActionResult> Delete(int id)
        {
            var propertyTypeVm = await _propertyTypeService.GetByIdSaveViewModel(id);
            return View("Delete", propertyTypeVm);
        }

        [HttpPost]
        public async Task<IActionResult> DeletePropType(int id)
        {
            await _propertyTypeService.Delete(id);
            return RedirectToRoute(new { controller = "PropertyType", action = "Index" });
        }
    }
}
