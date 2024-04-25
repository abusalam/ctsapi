using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CTS_BE.Helper
{
    public static class ChequeStatusManagerHelper
    {
        private static Dictionary<string, List<int>> _chequeIndentListStatusMap;
        private static Dictionary<string, int> _newChequeIndentStatusMap;
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
        public static int getStatus(string role)
        {
            return _newChequeIndentStatusMap[role];
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
                { "can-create-indent-invoice",
                    new List<int>
                    {
                        (int) Enum.IndentStatus.ApproveByTreasuryOfficer,
                    }
                },
            };
            _newChequeIndentStatusMap = new Dictionary<string, int>{
                {"cheque-operator", (int) Enum.IndentStatus.NewIndent},
                {"treasury-officer", (int) Enum.IndentStatus.ApproveByTreasuryOfficer},
                {"DTA", (int) Enum.IndentStatus.ApproveByTreasuryOfficer},
            };
        }
    }
}