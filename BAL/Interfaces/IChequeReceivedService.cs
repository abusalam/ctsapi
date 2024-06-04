using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CTS_BE.DTOs;

namespace CTS_BE.BAL.Interfaces
{
    public interface IChequeReceivedService
    {
       public Task<Int16?> ChequeReceived(ChequeReceivedDTO chequeReceivedDTO);
        public Task<IEnumerable<ChequeReceivedListDTO>> ChequeReceivedList(DynamicListQueryParameters dynamicListQueryParameters, List<int> statusIds);
        public  Task<IEnumerable<ChequeReceivedDataWithMICRDTO>> AllChequeReceivedList();
    }
}