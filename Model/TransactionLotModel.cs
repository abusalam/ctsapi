using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CTS_BE.Model
{
    public class TransactionLotModel
    {
        public long Id { get; set; }
        public string? LotNo { get; set; }
        public string? DrnNo { get; set; }
        public short? FinancialYearId { get; set; }
        public short? NumberOfBeneficiary { get; set; }
        public int? NumberOfSuccess { get; set; }
        public int? NumberOfFailed { get; set; }
        public int? TotalAmount { get; set; }
        public int? DebitAmount { get; set; }
        public short? Status { get; set; }
        public int? VoucherNo { get; set; }
        public DateOnly? VoucherDate { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}