using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DTOs;
using Microsoft.EntityFrameworkCore.Storage;

namespace CTS_BE.DAL.Interfaces.Pension
{
    public interface IManualPpoReceiptRepository : IRepository<PpoReceipt>
    {
        public Task<List<ListAllPpoReceiptsResponseDTO>> GetAllUnusedPpoReceipts(
            short financialYear,
            string treasuryCode,
            Expression<Func<PpoReceipt, ListAllPpoReceiptsResponseDTO>> selectExpression
        );
    }
}