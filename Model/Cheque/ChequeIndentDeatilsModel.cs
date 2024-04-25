using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CTS_BE.Model.Cheque
{
    public class ChequeIndentDeatilsModel
    {
        public short ChequeType { get; set; } 
        public string MicrCode { get; set; }    
        public int Quantity { get; set; }
    }
}