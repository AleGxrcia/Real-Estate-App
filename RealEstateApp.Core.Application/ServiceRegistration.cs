using RealEstateApp.Core.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RealEstateApp.Core.Application.Interfaces.Services;
using System.Reflection;
using MediatR;

namespace RealEstateApp.Core.Application
{

    //Extension Method - Decorator
    public static class ServiceRegistration
    {
        public static void AddApplicationLayer(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());
            #region Services
            services.AddTransient(typeof(IGenericService<,,>), typeof(GenericService<,,>));
            services.AddTransient<IPropertyTypeService, PropertyTypeService>();
            services.AddTransient<ISaleTypeService, SaleTypeService>();
            services.AddTransient<IImprovementService, ImprovementService>();
			services.AddTransient<IPropertyService, PropertyService>();
			services.AddTransient<IUserService, UserService>();



			#endregion
		}
    }
}
