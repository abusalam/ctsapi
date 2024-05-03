using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CTS_BE.Model.Cheque
{
    public class ChequeEntryModel
    {
        public short FinancialYearId { get; set; }
        public string TreasurieCode { get; set; }
        public string MicrCode { get; set; }
        public string SeriesNo { get; set; }
        public short Start { get; set; }
        public short End { get; set; }
        public short Quantity { get; set; }
        public long CreatedBy { get; set; }
    }
}