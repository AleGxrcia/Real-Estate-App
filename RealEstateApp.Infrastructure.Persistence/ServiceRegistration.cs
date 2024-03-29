using RealEstateApp.Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Infrastructure.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Infrastructure.Persistence
{

    //Extension Method - Decorator
    public static class ServiceRegistration
    {
        public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            #region Contexts
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<ApplicationContext>(options => options.UseInMemoryDatabase("ApplicationDb"));
            }
            else
            {
                services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                m => m.MigrationsAssembly(typeof(ApplicationContext).Assembly.FullName)));
            }
            #endregion

            #region Repositories
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            services.AddTransient<IImprovementRepository, ImprovementRepository>();
            services.AddTransient<IPropertyRepository, PropertyRepository>();
            services.AddTransient<IPropertyTypeRepository, PropertyTypeRepository>();
            services.AddTransient<ISaleTypeRepository, SaleTypeRepository>();


            #endregion
        }
    }
}
