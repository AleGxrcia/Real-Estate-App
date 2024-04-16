using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.ViewModels.SaleType;

namespace RealEstateApp.WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SaleTypeController : Controller
    {
        private readonly ISaleTypeService _saleTypeService;

        public SaleTypeController(ISaleTypeService saleTypeService)
        {
            _saleTypeService = saleTypeService;
        }

        public async Task<IActionResult> Index()
        {
            var saleTypes = await _saleTypeService.GetAllViewModelWithInclude();
            return View(saleTypes);
        }

        public IActionResult Create()
        {
            return View("SaveSaleType", new SaveSaleTypeViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaveSaleTypeViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("SaveSaleType", vm);
            }

            await _saleTypeService.Add(vm);

            return RedirectToRoute(new { controller = "SaleType", action = "Index" });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var saleTypeVm = await _saleTypeService.GetByIdSaveViewModel(id);
            return View("SaveSaleType", saleTypeVm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SaveSaleTypeViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("SaveSaleType", vm);
            }

            await _saleTypeService.Update(vm, vm.Id);

            return RedirectToRoute(new { controller = "SaleType", action = "Index" });
        }

        public async Task<IActionResult> Delete(int id)
        {
            var saleTypeVm = await _saleTypeService.GetByIdSaveViewModel(id);
            return View("Delete", saleTypeVm);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSaleType(int id)
        {
            await _saleTypeService.Delete(id);
            return RedirectToRoute(new { controller = "SaleType", action = "Index" });
        }
    }
}
