using AutoMapper;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.ViewModels.Improvement;
using RealEstateApp.Core.Application.ViewModels.Property;
using RealEstateApp.Core.Domain.Entities;
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
				ImagesUrl = p.Images.Where(img => img.ImageUrl != null).Select(img => img.ImageUrl).ToList(),
				Improvements = p.ImprovementProperties.Where(pi => pi.Improvement != null).Select(i => new ImprovementViewModel
				{
					Name = i.Improvement.Name,
					Description = i.Improvement.Description

				}).ToList()
			}).ToList();

			if (filters.PropertyType != null && filters.MinPrice != null && filters.MaxPrice != null && filters.NumberOfBathRooms != null && filters.NumberOfBathRooms != null)
			{
				propertiesWithFilters = propertiesWithFilters.Where(p => p.PropertyType == filters.PropertyType
				&& p.Price >= filters.MinPrice && p.Price <= filters.MaxPrice
				&& p.NumberOfBathrooms == filters.NumberOfBathRooms
				&& p.NumberOfRooms == filters.NumberOfRooms)
				.ToList();

			}
			else if (filters.PropertyType != null)
			{
				propertiesWithFilters = propertiesWithFilters.Where(p => p.PropertyType == filters.PropertyType).ToList();
			}
			else if (filters.MinPrice != null)
			{
				propertiesWithFilters = propertiesWithFilters.Where(p => p.Price >= filters.MinPrice).ToList();
			}
			else if (filters.MaxPrice != null)
			{
				propertiesWithFilters = propertiesWithFilters.Where(p => p.Price <= filters.MaxPrice).ToList();
			}
			else if (filters.MaxPrice != null && filters.MinPrice != null)
			{
				propertiesWithFilters = propertiesWithFilters.Where(p => p.Price >= filters.MinPrice && p.Price <= filters.MaxPrice).ToList();
			}
			else if (filters.NumberOfRooms != null) 
			{
				propertiesWithFilters = propertiesWithFilters.Where(p => p.NumberOfRooms == filters.NumberOfRooms).ToList();
			}
			else if (filters.NumberOfBathRooms != null)
			{
				propertiesWithFilters = propertiesWithFilters.Where(p => p.NumberOfBathrooms == filters.NumberOfBathRooms).ToList();
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
	var propertyList = await _repository.GetAllWithIncludeAsync(new List<string> { "SaleType", "PropertyType", "Images", "FavoriteProperties", "ImprovementProperties" });
	var property = propertyList.FirstOrDefault(x => x.Id == Id);
	var Agent = await _userService.getUserByIdAsync(property.AgentId);
	if (property == null)
	{
		return null;
	}

	var images = property.Images?.ToList();

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
		ImproveMentes = _mapper.Map<List<ImprovementViewModel>>(property.ImprovementProperties),
		ImgUrl1 = images.Count > 0 ? images[0].ImageUrl : null,
		ImgUrl2 = images.Count > 1 ? images[1].ImageUrl : null,
		ImgUrl3 = images.Count > 2 ? images[2].ImageUrl : null,
		ImgUrl4 = images.Count > 3 ? images[3].ImageUrl : null,
		AgentName = Agent.FirstName,
		AgentEmail = Agent.Email,
		AgentPhoneNumber = Agent.Phone,
		AgentImgUrl = Agent.ImgUrl
	};

	return viewModel;
}
    }
}
