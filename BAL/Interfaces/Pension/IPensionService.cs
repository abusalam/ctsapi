using CTS_BE.DTOs.PensionDTO;

namespace CTS_BE.BAL.Interfaces.Pension
{
    public interface IPensionService
    {
        public Task<int> CreatePensionerDetails(PensionerDetailsDTO details);
    }
}