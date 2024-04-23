using AutoMapper;
using CTS_BE.BAL.Interfaces;
using CTS_BE.DAL.Entities;
using CTS_BE.DAL.Interfaces;
using CTS_BE.DTOs;
namespace CTS_BE.BAL
{
    public class TreasuryService : ITreasuryService
    {
        private readonly ITreasuryRepository _TreasuryRepository;
        private readonly IMapper _mapper;
        public TreasuryService(ITreasuryRepository TreasuryRepository, IMapper mapper) {
            _TreasuryRepository = TreasuryRepository;
            _mapper = mapper;
        }
        public async Task<List<DropdownStringCodeDTO>> GetTreasurys() 
        {
            return (List<DropdownStringCodeDTO>) await _TreasuryRepository.GetSelectedColumnAsync(entity => new DropdownStringCodeDTO
            {
                Name = entity.Name,
                Code = entity.Code
            });
        }
    }
}