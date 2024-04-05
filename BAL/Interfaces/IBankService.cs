using CTS_BE.DTOs;

namespace CTS_BE.BAL.Interfaces
{
    public interface IBankService
    {
        public Task<IEnumerable<DropdownDTO>> AllBanks();
    }
}