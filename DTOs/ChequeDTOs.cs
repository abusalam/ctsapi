using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CTS_BE.DTOs
{
    public class ChequeListDTOs
    {
        public string PayeeName { get; set; }
        public decimal Amount { get; set; }
        public string	ChequeType { get; set; }
    }
}