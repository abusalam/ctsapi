using AutoMapper;
using CTS_BE.BAL.Interfaces;
using CTS_BE.DAL.Entities;
using CTS_BE.DAL.Interfaces;
using CTS_BE.DTOs;

namespace CTS_BE.BAL
{
    public class BankService : IBankService
    {
        private readonly IBankRepository _BankRepository;
        private readonly IMapper _mapper;
        public BankService(IBankRepository BankRepository, IMapper mapper) {
            _BankRepository = BankRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<DropdownDTO>> AllBanks()
        {
            IEnumerable<DropdownDTO> banksDropdownData = (IEnumerable<DropdownDTO>)await  _BankRepository.GetSelectedColumnByConditionAsync(entity=>entity.IsActive,entity=> new DropdownDTO
            {
                Name = entity.BankName,
                Code = entity.BankCode,
            });
            return banksDropdownData;
        }
    }
}