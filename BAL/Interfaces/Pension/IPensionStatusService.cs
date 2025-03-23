using CTS_BE.DTOs;
using CTS_BE.PensionEnum;

namespace CTS_BE.BAL.Interfaces.Pension
{
    public interface IPensionStatusService
    {
        public Task<T> SetPensionStatusFlag<T>(
            PensionStatusEntryDTO pensionStatusEntryDTO,
            short financialYear,
            string treasuryCode
        );
        public Task<PensionStatusDTO> ClearPensionStatusFlag(
            int ppoId,
            PensionStatusFlag pensionStatusFlag,
            short financialYear,
            string treasuryCode
        );
        public Task<PensionStatusDTO> CheckPensionStatusFlag(
            int ppoId,
            PensionStatusFlag pensionStatusFlag,
            short financialYear,
            string treasuryCode
        );
    }
}
