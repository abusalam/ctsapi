using CTS_BE.DTOs;

namespace CTS_BE.BAL.Interfaces
{
    public interface IChequeCountService
    {
        public Task<IEnumerable<DropdownStringCodeDTO>> GetAvailableChequeMICRByTreasuryCode(string treasuryCode);
    }
}