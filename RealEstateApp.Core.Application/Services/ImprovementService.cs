using AutoMapper;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.ViewModels.Improvement;
using RealEstateApp.Core.Domain.Entities;

namespace RealEstateApp.Core.Application.Services
{
    public class ImprovementService : GenericService<SaveImprovementViewModel, ImprovementViewModel, Improvement>, IImprovementService
    {
        private readonly IImprovementRepository _improvementRepository;
        private readonly IMapper _mapper;

        public ImprovementService(IImprovementRepository improvementRepository, IMapper mapper) : base(improvementRepository, mapper)
        {
            _improvementRepository = improvementRepository;
            _mapper = mapper;
        }
    }
}
