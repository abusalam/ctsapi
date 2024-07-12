using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CTS_BE.DTOs
{
    public class BeneficiaryDetailsDTO
    {
           public string? BeneficiaryName { get; set; }
           public string? BeneficiaryCode {get; set;}
           public string? IFSCCode {get; set;}
           public string? BankName {get; set;}
           public string? AccountNumber {get; set;}
           public string? EMail {get; set;}
            public string? ContactNo {get; set;}
           public decimal? Amount {get; set;}
           public string? PayeeType {get; set;}
    }
}