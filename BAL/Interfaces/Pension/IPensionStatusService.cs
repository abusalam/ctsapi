using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CTS_BE.DTOs;
using CTS_BE.PensionEnum;

namespace CTS_BE.BAL.Interfaces.Pension
{
    public interface IPensionStatusService : IBaseService
    {
        public Task<PensionStatusEntryDTO> SetPensionStatusFlag(
            PensionStatusEntryDTO pensionStatusEntryDTO,
            short financialYear,
            string treasuryCode
        );
        public Task<PensionStatusDTO> ClearPensionStatusFlag(
            int ppoId,
            int pensionStatusFlag,
            short financialYear,
            string treasuryCode
        );
        public Task<PensionStatusDTO> CheckPensionStatusFlag(
            int ppoId,
            int pensionStatusFlag,
            short financialYear,
            string treasuryCode
        );
    }
}