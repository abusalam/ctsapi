using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CTS_BE.Helper
{
    public static class ChequeStatusManagerHelper
    {
        private static Dictionary<string, List<int>> _chequeIndentListStatusMap;
         static ChequeStatusManagerHelper(){
            InitChequeIndentStatus();
        }
        public static List<int> getStatus(List<string> permissions)
        {
            return permissions
            .Where(key => _chequeIndentListStatusMap.ContainsKey(key))
            .SelectMany(key => _chequeIndentListStatusMap[key])
            .Distinct()
            .ToList();
        }
        private static void InitChequeIndentStatus()
        {
            _chequeIndentListStatusMap = new Dictionary<string, List<int>>
            {
                { "can-create-cheque-indent",
                    new List<int>
                    {
                        (int) Enum.IndentStatus.NewIndent,
                        (int) Enum.IndentStatus.FrowardToTreasuryOfficer,
                        (int) Enum.IndentStatus.ApproveByTreasuryOfficer,
                        (int) Enum.IndentStatus.RejectByTreasuryOfficer
                    }
                },
                { "can-approve-reject-cheque-indent",
                    new List<int>
                    {
                        (int) Enum.IndentStatus.ApproveByTreasuryOfficer,
                        (int) Enum.IndentStatus.FrowardToTreasuryOfficer,
                        (int) Enum.IndentStatus.RejectByDTA,
                    }
                },
                // { "can-create-cheque-inoice",
                //     new List<int>
                //     {
                        
                //     }
                // },
            };
        }
    }
}