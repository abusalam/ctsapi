using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CTS_BE.DTOs
{
    public class ByTransferDTO
    {
       public int? BtSerial { get; set; }
       public string? Hoa { get; set; }
       public string? Desc { get; set; }
       public string? Type { get; set; }
       public decimal? Amount { get; set; }
    }
}