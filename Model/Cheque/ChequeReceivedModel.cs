using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CTS_BE.Model.Cheque
{
    public class ChequeReceivedModel
    {
        public long Id { get; set; }
        public int Start { get; set; }
        public int End { get; set; }
        public int Quantity { get; set; }
        public int ReceivedUser { get; set; }
        public int InvoiceId { get; set; }
        public int InvoiceDeatilsId { get; set; }
    }
}