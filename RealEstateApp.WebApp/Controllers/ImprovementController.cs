using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.ViewModels.Improvement;

namespace RealEstateApp.WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ImprovementController : Controller
    {
        private readonly IImprovementService _improvementService;

        public ImprovementController(IImprovementService improvementService)
        {
            _improvementService = improvementService;
        }

        public async Task<IActionResult> Index()
        {
            var improvements = await _improvementService.GetAllViewModel();
            return View(improvements);
        }

        public IActionResult Create()
        {
            return View("SaveImprovement", new SaveImprovementViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaveImprovementViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("SaveImprovement", vm);
            }

            await _improvementService.Add(vm);

            return RedirectToRoute(new { controller = "Improvement", action = "Index" });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var improvementVm = await _improvementService.GetByIdSaveViewModel(id);
            return View("SaveImprovement", improvementVm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SaveImprovementViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("SaveImprovement", vm);
            }

            await _improvementService.Update(vm, vm.Id);

            return RedirectToRoute(new { controller = "Improvement", action = "Index" });
        }

        public async Task<IActionResult> Delete(int id)
        {
            var improvementVm = await _improvementService.GetByIdSaveViewModel(id);
            return View("Delete", improvementVm);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteImprovement(int id)
        {
            await _improvementService.Delete(id);
            return RedirectToRoute(new { controller = "Improvement", action = "Index" });
        }
    }
}
