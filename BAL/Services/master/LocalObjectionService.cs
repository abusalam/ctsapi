using AutoMapper;
using CTS_BE.BAL.Interfaces.master;
using CTS_BE.DAL.Entities;
using CTS_BE.DAL.Interfaces.master;
using CTS_BE.DTOs;

namespace CTS_BE.BAL.Services.master
{
    public class LocalObjectionService : ILocalObjectionService
    {
        private readonly ILocalObjectionRepository _LocalObjectionRepository;
        private readonly IMapper _mapper;
        public LocalObjectionService(ILocalObjectionRepository LocalObjectionRepository, IMapper mapper)
        {
            _LocalObjectionRepository = LocalObjectionRepository;
            _mapper = mapper;
        }
        public async Task<List<ObjectionDto>> AllObjections()
        {
            List<ObjectionDto> objections = (List<ObjectionDto>)await _LocalObjectionRepository.GetSelectedColumnAsync(
                 entity => new ObjectionDto
                 {
                     Id = entity.Id,
                     Description = entity.Description,
                 });
            return objections;
        }
        public async Task<bool> Insert(string description)
        {
            LocalObjection localObjection = new LocalObjection
            {
                Description = description,
                Status = 0,
                TreasuryCode = "AAA",
            };
            if (_LocalObjectionRepository.Add(localObjection))
            {
                _LocalObjectionRepository.SaveChangesManaged();
                return true;
            }
            return false;
        }
    }
}