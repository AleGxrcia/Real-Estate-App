﻿using RealEstateApp.Core.Domain.Entities;

namespace RealEstateApp.Core.Application.Interfaces.Repositories
{
    public interface IImprovementRepository : IGenericRepository<Improvement>
    {
        Task<List<Improvement>> GetAllByIdAsync(List<int> improvementIds);
    }
}
