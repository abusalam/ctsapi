using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CTS_BE.Model.Cheque;

namespace CTS_BE.Model
{
    public class ChequeIndentModel
    {
        public string IndentDate { get; set; }
        public string MemoNumber { get; set; }
        public string MemoDate { get; set; }
        public string Remarks { get; set; }
        public int Status { get; set; }
        public long UserId { get; set; }
        public List<ChequeIndentDeatilsModel> ChequeIndentDeatils { get; set; }
    }
}