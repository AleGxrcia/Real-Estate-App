using AutoMapper;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.ViewModels.Improvement;
using RealEstateApp.Core.Application.ViewModels.Property;
using RealEstateApp.Core.Domain.Entities;
using System.Reflection.Metadata.Ecma335;
using static System.Net.Mime.MediaTypeNames;

namespace RealEstateApp.Core.Application.Services
{
    public class PropertyService : GenericService<SavePropertyViewModel, PropertyViewModel, Property>, IPropertyService
    {
        private readonly IPropertyRepository _repository;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public PropertyService(IPropertyRepository repository, IMapper mapper, IUserService userService) : base(repository, mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _userService = userService;
        }



public async Task AddImagesAsync(List<string> photoUrls, int propertyId)
{
	await _repository.AddImagesAsync(photoUrls, propertyId);
}

public async Task AddImprovementToPropertyAsync(List<int> improvementsId, int propertyId)
{
	await _repository.AddImprovementToPropertyAsync(improvementsId, propertyId);
}

public async Task<List<PropertyViewModel>> GetAllPropertiesByAgentId(string Id)
{
	var propertyList = await GetAllWithIncludeAsync();
	var AgentProperties = propertyList.Where(p => p.AgentId == Id)
									  .Select(p => new PropertyViewModel
									  {
										  Id = p.Id,
										  PropertyType = p.PropertyType,
										  Code = p.Code,
										  SaleType = p.SaleType,
										  Price = p.Price,
										  LandSize = p.LandSize,
										  NumberOfBathrooms = p.NumberOfBathrooms,
										  NumberOfRooms = p.NumberOfRooms,
										  AgentId = p.AgentId,
										  ImagesUrl = p.ImagesUrl
									  })
									  .ToList();

	return AgentProperties;
}

		public async Task<List<PropertyViewModel>> GetAllWithFilters(FiltersPropertyViewModel filters)
		{
			var propertyList = await _repository.GetAllWithIncludeAsync(new List<string> { "SaleType", "PropertyType", "Images", "FavoriteProperties", "ImprovementProperties" });
			var propertiesWithFilters = propertyList.Select(p => new PropertyViewModel
			{
				Id = p.Id,
				PropertyType = p.PropertyType.Name,
				Code = p.Code,
				SaleType = p.SaleType.Name,
				Price = p.Price,
				LandSize = p.LandSize,
				NumberOfBathrooms = p.NumberOfBathrooms,
				NumberOfRooms = p.NumberOfRooms,
				AgentId = p.AgentId,
				PropertyTypeId = p.PropertyType.Id,
				ImagesUrl = p.Images.Where(img => img.ImageUrl != null).Select(img => img.ImageUrl).ToList(),
				Improvements = p.ImprovementProperties.Where(pi => pi.Improvement != null).Select(i => new ImprovementViewModel
				{
					Name = i.Improvement.Name,
					Description = i.Improvement.Description

				}).ToList()
			}).ToList();

            if (filters != null)
            {
                propertiesWithFilters = propertiesWithFilters.Where(p =>
                    (filters.PropertyType == null || p.PropertyTypeId == filters.PropertyType) &&
                    (filters.MinPrice == null || p.Price >= filters.MinPrice) &&
                    (filters.MaxPrice == null || p.Price <= filters.MaxPrice) &&
                    (filters.NumberOfBathRooms == null || p.NumberOfBathrooms == filters.NumberOfBathRooms) &&
                    (filters.NumberOfRooms == null || p.NumberOfRooms == filters.NumberOfRooms))
                .ToList();
            }



            return propertiesWithFilters;
		}


		public async Task<List<PropertyViewModel>> GetAllWithIncludeAsync()
{
	var propertyList = await _repository.GetAllWithIncludeAsync(new List<string> { "SaleType", "PropertyType", "Images", "FavoriteProperties", "ImprovementProperties" });
	return propertyList.Select(p => new PropertyViewModel
	{
		Id = p.Id,
		PropertyType = p.PropertyType.Name,
		Code = p.Code,
		SaleType = p.SaleType.Name,
		Price = p.Price,
		LandSize = p.LandSize,
		NumberOfBathrooms = p.NumberOfBathrooms,
		NumberOfRooms = p.NumberOfRooms,
		AgentId = p.AgentId,
		ImagesUrl = p.Images.Where(img => img.ImageUrl != null).Select(img => img.ImageUrl).ToList(),
        Improvements = p.ImprovementProperties.Where(pi => pi.Improvement != null).Select(i=>new ImprovementViewModel 
		{
			Name = i.Improvement.Name,
			Description = i.Improvement.Description
			
		}).ToList()
	}).ToList();
}

public async Task<PropertyDetailsViewModel> GetPropertyDetails(int Id)
{
	var propertyList = await _repository.GetAllWithIncludeAsync(new List<string> { "SaleType", "PropertyType", "Images", "FavoriteProperties", "ImprovementProperties.Improvement" });
	var property = propertyList.FirstOrDefault(x => x.Id == Id);
	var Agent = await _userService.getUserByIdAsync(property.AgentId);
	if (property == null)
	{
		return null;
	}

	var images = property.Images?.ToList();

	 // Obtener los mejoramientos (improvements)
    var improvements = property.ImprovementProperties
        .Where(pi => pi.Improvement != null)
        .Select(i => new ImprovementViewModel
        {
            Name = i.Improvement.Name,
            Description = i.Improvement.Description
        }).ToList();


	var viewModel = new PropertyDetailsViewModel
	{
		Id = property.Id,
		PropertyType = property.PropertyType?.Name,
		Code = property.Code,
		SalesType = property.SaleType?.Name,
		Price = property.Price,
		LandSize = property.LandSize,
		NumberOfBathrooms = property.NumberOfBathrooms,
		NumberOfRooms = property.NumberOfRooms,
		Description = property.Description,
		ImproveMentes = improvements,
		ImgUrl1 = images.Count > 0 ? images[0].ImageUrl : null,
		ImgUrl2 = images.Count > 1 ? images[1].ImageUrl : null,
		ImgUrl3 = images.Count > 2 ? images[2].ImageUrl : null,
		ImgUrl4 = images.Count > 3 ? images[3].ImageUrl : null,
		AgentName = Agent.FirstName,
		AgentEmail = Agent.Email,
		AgentPhoneNumber = Agent.Phone,
		AgentImgUrl = Agent.PhotoUrl
    };

	return viewModel;
}

        public async Task UpdateImagesAsync(List<string> photoUrls, int propertyId)
        {
			await _repository.UpdateImagesAsync(photoUrls, propertyId);
        }

        public async Task UpdateImprovementsAsync(List<int> improvementsId, int propertyId)
        {
			await _repository.UpdateImprovementsAsync(improvementsId, propertyId);
        }

		
        public override async Task<SavePropertyViewModel> GetByIdSaveViewModel(int id)
        {
			var propertyList = await _repository.GetAllWithIncludeAsync(new List<string> { "SaleType", "PropertyType", "Images", "FavoriteProperties", "ImprovementProperties" });
            var p = propertyList.FirstOrDefault(x => x.Id == id);

            if (p == null)
            {
                return null;
            }

            var images = p.Images?.ToList();

			var viewmodel = new SavePropertyViewModel
			{
				Id = p.Id,
				PropertyTypeId = p.PropertyType.Id,
				Code = p.Code,
				SaleTypeId = p.SaleType.Id,
				Price = p.Price,
				LandSize = p.LandSize,
				NumberOfBathrooms = p.NumberOfBathrooms,
				NumberOfRooms = p.NumberOfRooms,
				Description = p.Description,
				AgentId = p.AgentId,
				ImgUrl1 = images.Count > 0 ? images[0].ImageUrl : null,
				ImgUrl2 = images.Count > 1 ? images[1].ImageUrl : null,
				ImgUrl3 = images.Count > 2 ? images[2].ImageUrl : null,
				ImgUrl4 = images.Count > 3 ? images[3].ImageUrl : null,
				Improvements = p.ImprovementProperties.Where(pi => pi.Improvement != null).Select(i => new ImprovementViewModel
				{
					Name = i.Improvement.Name,
					Description = i.Improvement.Description

				}).ToList()
			};

			return viewmodel;
        }

        public async Task DeleteImprovementPropertiesAsync(int propertyId)
        {
			await _repository.DeleteImprovementPropertiesAsync(propertyId);
        }
    }
}
