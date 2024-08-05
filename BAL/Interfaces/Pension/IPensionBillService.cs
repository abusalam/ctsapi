using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CTS_BE.DTOs;

namespace CTS_BE.BAL.Interfaces.Pension
{
    public interface IPensionBillService : IBaseService
    {
        public Task<InitiateFirstPensionBillResponseDTO> GenerateFirstPensionBills(
            InitiateFirstPensionBillDTO initiateFirstPensionBillDTO,
            short financialYear,
            string treasuryCode
        );
    }
}