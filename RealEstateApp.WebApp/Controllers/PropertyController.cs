using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Dtos.Account;
using RealEstateApp.Core.Application.Helpers;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.Services;
using RealEstateApp.Core.Application.ViewModels.Improvement;
using RealEstateApp.Core.Application.ViewModels.Property;
using RealEstateApp.Core.Application.ViewModels.PropertyType;
using System.Text;

namespace RealEstateApp.WebApp.Controllers
{
	[Authorize(Roles = "Agent")]
	public class PropertyController : Controller
	{
		private readonly IPropertyService _propertyService;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly AuthenticationResponse user;
		private readonly IPropertyTypeService _propertyTypeService;
        private readonly IImprovementService _improvementService;
		private readonly ISaleTypeService _saleTypeService;
        public PropertyController(IPropertyService propertyService, IHttpContextAccessor httpContextAccessor, IPropertyTypeService propertyTypeService, IImprovementService improvementService, ISaleTypeService saleTypeService)
		{
			_propertyService = propertyService;
            _httpContextAccessor = httpContextAccessor;
            user = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
			_propertyTypeService = propertyTypeService;
			_improvementService = improvementService;
			_saleTypeService = saleTypeService;
		}

        public async Task<IActionResult> Index()
        {
            var properties = await _propertyService.GetAllPropertiesByAgentId(user.Id);
            properties.Reverse();
            return View(properties);
        }



        //Crear Propiedad
        public async Task<IActionResult> Create()
		{
			SavePropertyViewModel vm = new();
			vm.PropertyTypes = await _propertyTypeService.GetAllViewModel();
			vm.SalesTypes = await _saleTypeService.GetAllViewModel();
			vm.Improvements = await _improvementService.GetAllViewModel();
			return View("SaveProperty", vm);
		}

		[HttpPost]
		public async Task<IActionResult> Create(SavePropertyViewModel vm)
		{
			if (!ModelState.IsValid)
			{
                vm.PropertyTypes = await _propertyTypeService.GetAllViewModel();
                vm.SalesTypes = await _saleTypeService.GetAllViewModel();
                vm.Improvements = await _improvementService.GetAllViewModel();

                return View("SaveProperty", vm);
			}


			vm.AgentId = user.Id;
            vm.Code = CodeGeneratorHelper.GeneratePropertyCode();
            SavePropertyViewModel Property = await _propertyService.Add(vm);
            vm.ImgUrl1 = FileManagerHelper.UploadFile(vm.file1, Property.Id, "PropertyImages");
            vm.ImgUrl2 = vm.file2 != null? FileManagerHelper.UploadFile(vm.file2, Property.Id, "PropertyImages") : "";
            vm.ImgUrl3 = vm.file3 != null? FileManagerHelper.UploadFile(vm.file3, Property.Id, "PropertyImages") : "";
            vm.ImgUrl4 = vm.file4 != null? FileManagerHelper.UploadFile(vm.file4, Property.Id, "PropertyImages") : "";

            List<string> Images = new List<string>();
            Images.Add(vm.ImgUrl1);
            if (vm.ImgUrl2 != "")
            {
                Images.Add(vm.ImgUrl2);
            }

            if (vm.ImgUrl3 != "")
            {
                Images.Add(vm.ImgUrl3);
            }

            if (vm.ImgUrl4 != "")
            {
                Images.Add(vm.ImgUrl4);
            }


            await _propertyService.AddImprovementToPropertyAsync(vm.ImprovementsId, Property.Id);
			await _propertyService.AddImagesAsync(Images, Property.Id);

			return RedirectToRoute(new { controller = "Property", action = "Index" });
		}

		//Editar propiedad
        public async Task<IActionResult> Edit(int id)
        {
            var propertyVm = await _propertyService.GetByIdSaveViewModel(id);
            propertyVm.PropertyTypes = await _propertyTypeService.GetAllViewModel();
            propertyVm.SalesTypes = await _saleTypeService.GetAllViewModel();
            propertyVm.Improvements = await _improvementService.GetAllViewModel();
            return View("SaveProperty", propertyVm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SavePropertyViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.PropertyTypes = await _propertyTypeService.GetAllViewModel();
                vm.SalesTypes = await _saleTypeService.GetAllViewModel();
                vm.Improvements = await _improvementService.GetAllViewModel();
                return View("SaveProperty", vm);
            }

			vm.AgentId = user.Id;
            var property = await _propertyService.GetByIdSaveViewModel(vm.Id);
            vm.Code = property.Code;
            await _propertyService.Update(vm, vm.Id);

            if (property.ImgUrl1 == null)
            {
                vm.ImgUrl1 = vm.file1 != null ? FileManagerHelper.UploadFile(vm.file1, vm.Id, "PropertyImages") : "";
            }
            else
            {
                vm.ImgUrl1 = FileManagerHelper.UploadFile(vm.file1, vm.Id, "PropertyImages", true, property.ImgUrl1);
            }

            if (property.ImgUrl2 == null)
            {
                vm.ImgUrl2 = vm.file2 != null ? FileManagerHelper.UploadFile(vm.file2, vm.Id, "PropertyImages") : "";
            }
            else
            {
                vm.ImgUrl2 = FileManagerHelper.UploadFile(vm.file2, vm.Id, "PropertyImages", true, property.ImgUrl2);
            }

            if (property.ImgUrl3 == null)
            {
                vm.ImgUrl3 = vm.file3 != null ? FileManagerHelper.UploadFile(vm.file3, vm.Id, "PropertyImages") : "";
            }
            else
            {
                vm.ImgUrl3 = FileManagerHelper.UploadFile(vm.file3, vm.Id, "PropertyImages", true, property.ImgUrl3);
            }

            if (property.ImgUrl4 == null)
            {
                vm.ImgUrl4 = vm.file4 != null ? FileManagerHelper.UploadFile(vm.file4, vm.Id, "PropertyImages") : "";
            }
            else
            {
                vm.ImgUrl4 = FileManagerHelper.UploadFile(vm.file4, vm.Id, "PropertyImages", true, property.ImgUrl4);
            }

            List<string> Images = new List<string>();
            if (vm.ImgUrl1 != "")
            {
                Images.Add(vm.ImgUrl1);
            }

            if (vm.ImgUrl2 != "")
            {
                Images.Add(vm.ImgUrl2);
            }

            if (vm.ImgUrl3 != "")
            {
                Images.Add(vm.ImgUrl3);
            }

            if (vm.ImgUrl4 != "")
            {
                Images.Add(vm.ImgUrl4);
            }


            await _propertyService.UpdateImprovementsAsync(vm.ImprovementsId, vm.Id);
            if (Images.Count != 0) 
            {
                await _propertyService.UpdateImagesAsync(Images, vm.Id);
            }
            

            return RedirectToRoute(new { controller = "Property", action = "Index" });
        }

        //Eliminar propiedad
        public async Task<IActionResult> Delete(int id)
        {
            var propertyeVm = await _propertyService.GetByIdSaveViewModel(id);
            return View("Delete", propertyeVm);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProperty(int id)
        {
            await _propertyService.DeleteImprovementPropertiesAsync(id);
            await _propertyService.Delete(id);

            FileManagerHelper.DeleteFile(id, "PropertyImages");

            return RedirectToRoute(new { controller = "Property", action = "Index" });
        }

        //GenerarCodigo
        private int GeneratePropertyCode()
        {
            Random random = new Random();
            StringBuilder propertyCode = new StringBuilder();

            for (int i = 0; i < 5; i++)
            {
                propertyCode.Append(random.Next(0, 10)); // Genera un dígito aleatorio entre 0 y 9
            }

            return int.Parse(propertyCode.ToString());
        }

    }
}
