using CTS_BE.DTOs;

namespace CTS_BE.BAL.Interfaces
{
    public interface ITreasuryService
    {
        public Task<List<DropdownStringCodeDTO>> GetTreasurys();
    }
}