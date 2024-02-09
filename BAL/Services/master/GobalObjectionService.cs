using AutoMapper;
using CTS_BE.BAL.Interfaces.master;
using CTS_BE.DAL.Entities;
using CTS_BE.DAL.Interfaces.master;
using CTS_BE.DTOs;

namespace CTS_BE.BAL.Services.master
{
    public class GobalObjectionService : IGobalObjectionService
    {
        private readonly IGobalObjectionRepository _GobalObjectionRepository;
        private readonly IMapper _mapper;
        public GobalObjectionService(IGobalObjectionRepository GobalObjectionRepository, IMapper mapper)
        {
            _GobalObjectionRepository = GobalObjectionRepository;
            _mapper = mapper;
        }
        public async Task<List<ObjectionDto>> AllObjections()
        {
           List<ObjectionDto> objections = (List<ObjectionDto>) await _GobalObjectionRepository.GetSelectedColumnByConditionAsync(entity=>entity.Status==1,
                entity=> new ObjectionDto
                {
                    Id = entity.Id,
                    Description = entity.Description,
                    ObjectionType = "Global"
                });
            return objections;
        }
    }
}