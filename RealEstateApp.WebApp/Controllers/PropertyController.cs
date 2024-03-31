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
	[Authorize(Roles = "Admin")]
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
			user = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
			_propertyTypeService = propertyTypeService;
			_improvementService = improvementService;
			_saleTypeService = saleTypeService;
		}

		public async Task<IActionResult> Index()
		{
			return View(await _propertyService.GetAllPropertiesByAgentId(user.Id));
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
				return View("SaveProperty", vm);
			}
            

			vm.AgentId = user.Id;
            vm.Code = GeneratePropertyCode();
            SavePropertyViewModel Property = await _propertyService.Add(vm);
            vm.ImgUrl1 = UploadFile(vm.file1, Property.Id);
            vm.ImgUrl2 = vm.ImgUrl2 != null? UploadFile(vm.file2, Property.Id) : "";
            vm.ImgUrl3 = vm.ImgUrl3 != null? UploadFile(vm.file3, Property.Id) : "";
            vm.ImgUrl4 = vm.ImgUrl3 != null? UploadFile(vm.file4, Property.Id) : "";

            List<string> Images = new List<string>()
            {
                vm.ImgUrl1,
                vm.ImgUrl2,
                vm.ImgUrl3,
                vm.ImgUrl4
            };

            await _propertyService.AddImprovementToPropertyAsync(vm.ImprovementsId, Property.Id);
			await _propertyService.AddImagesAsync(Images, Property.Id);

			return RedirectToRoute(new { controller = "Property", action = "Index" });
		}

		//Editar propiedad
        public async Task<IActionResult> Edit(int id)
        {
            var propertyVm = await _propertyService.GetByIdSaveViewModel(id);
            return View("SaveProperty", propertyVm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SavePropertyViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("SaveProperty", vm);
            }
			vm.AgentId = user.Id;
            await _propertyService.Update(vm, vm.Id);

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
            await _propertyService.Delete(id);
            return RedirectToRoute(new { controller = "Property", action = "Index" });
        }



        //UploadFile
        private string UploadFile(IFormFile file, int id, bool isEditMode = false, string imagePath = "")
        {
            if (isEditMode)
            {
                if (file == null)
                {
                    return imagePath;
                }
            }
            string basePath = $"/Images/ProperyImages/{id}";
            string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot{basePath}");

            //create folder if not exist
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            //get file extension
            Guid guid = Guid.NewGuid();
            FileInfo fileInfo = new(file.FileName);
            string fileName = guid + fileInfo.Extension;

            string fileNameWithPath = Path.Combine(path, fileName);

            using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            if (isEditMode)
            {
                string[] oldImagePart = imagePath.Split("/");
                string oldImagePath = oldImagePart[^1];
                string completeImageOldPath = Path.Combine(path, oldImagePath);

                if (System.IO.File.Exists(completeImageOldPath))
                {
                    System.IO.File.Delete(completeImageOldPath);
                }
            }
            return $"{basePath}/{fileName}";
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
