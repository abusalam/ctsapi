using AutoMapper;
using CTS_BE.BAL.Interfaces;
using CTS_BE.DAL.Entities;
using CTS_BE.DAL.Interfaces;
using CTS_BE.DTOs;
namespace CTS_BE.BAL
{
    public class ChequeCountService : IChequeCountService
    {
        private readonly IChequeCountRepository _ChequeCountRepository;
        private readonly IMapper _mapper;
        public ChequeCountService(IChequeCountRepository ChequeCountRepository, IMapper mapper)
        {
            _ChequeCountRepository = ChequeCountRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<DropdownStringCodeDTO>> GetAvailableChequeMICRByTreasuryCode(string treasuryCode)
        {
            return await _ChequeCountRepository.GetSelectedColumnByConditionAsync(entity => entity.TotalCount != entity.Utilized && entity.TreasuryCode == treasuryCode, entity => new DropdownStringCodeDTO
            {
                Name = entity.MicrCode,
                Code = entity.MicrCode
            });
        }
    }
}