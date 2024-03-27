using Microsoft.EntityFrameworkCore;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Infrastructure.Persistence.Contexts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RealEstateApp.Infrastructure.Persistence.Repository
{
    public class SaleTypeRepository : GenericRepository<SaleType>, ISaleTypeRepository
    {
        private readonly ApplicationContext _dbContext;

        public SaleTypeRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }


    }
}
